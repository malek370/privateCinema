using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using privateCinema.DataAccess;
using privateCinema.DTOs.MovieDTO;
using privateCinema.DTOs.ReservationDTOs;
using privateCinema.Models;
using System.Security.Claims;

namespace privateCinema.Services.ReservationService
{
    public class ReservationService : IResertvationService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ReservationService(CinemaDbContext cinemaDb, IMapper mapper, UserManager<IdentityUser> userManager,IHttpContextAccessor httpContext)
        {
            _context = cinemaDb;
            _mapper = mapper;
            _contextAccessor = httpContext;
            _userManager = userManager;
        }
        public async Task<Response<GetReservationDTO>> Creat(CreatReservationDTO creatreservation)
        {
            Response<GetReservationDTO> result = new Response<GetReservationDTO>();
            
            try
            {
                if (IsBefore10OrAfter2359(creatreservation.Time)) throw new Exception("invalid time");
                var reservation = new Reservation()
                {
                    User = await _userManager.FindByIdAsync(_contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    ReservatrionDate = creatreservation.Date.ToDateTime(creatreservation.Time),
                    Movie = await _context.Movies.FirstOrDefaultAsync(m=>m.Id==creatreservation.MovieId),
                    ReservatrionDateStart = creatreservation.Date.ToDateTime(creatreservation.Time).AddMinutes(-30),
                    ReservatrionDateEnd= creatreservation.Date.ToDateTime(creatreservation.Time).AddMinutes(30+ _context.Movies.FirstOrDefaultAsync(m => m.Id == creatreservation.MovieId).Result.Runtime)
                };
                //get the rooms that can accept the invited poeple( in terms of capacity)
                var rooms =await  _context.Rooms.
                    Include(r=>r.Reservations).
                    Where(r => r.Capacity >= creatreservation.NbInvited).
                    OrderBy(u=>u.Capacity*-1).
                    ToListAsync();
                //check the room availble
                var resroom=GetFirstRoom(rooms, reservation);
                if (resroom == null) throw new Exception("No possible room found");
                reservation.Room = resroom;
                result.data = new GetReservationDTO()
                {

                    Id = reservation.Id,
                    UserEmail = reservation.User!.Email!,
                    ReservationDate = reservation.ReservatrionDate,
                    MovieName = reservation.Movie!.Title!,
                    RoomName = reservation.Room!.Name,
                    State = reservation.state

                };
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> Delete(string reservationId)
        {
            var result = new Response<object>();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(u => u.Id == reservationId);
                if (reservation == null) { throw new Exception("reservation not found"); }
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<List<GetReservationDTO>>> GetAll()
        {
            var result = new Response<List<GetReservationDTO>>();
            try
            {
                List<GetReservationDTO> reservations;

                reservations = await _context.Reservations.
                    Include(u=>u.Movie).
                    Include(u=>u.Room).
                    Include(u=>u.User).
                    Select(u =>new GetReservationDTO()
                    {
                        Id = u.Id,
                        UserEmail=u.User!.Email!,
                        ReservationDate=u.ReservatrionDate,
                        MovieName=u.Movie!.Title!,
                        RoomName=u.Room!.Name,
                        State=u.state
                    } ).
                    ToListAsync();
                
                
                result.data = reservations;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<GetReservationDTO>> GetByID(string reservationId)
        {
            var result = new Response<GetReservationDTO>();
            try
            {
                var movie = await _context.Reservations.
                    Include(u => u.Movie).
                    Include(u => u.Room).
                    Include(u => u.User).
                    Select(u => new GetReservationDTO()
                {
                    Id = u.Id,
                    UserEmail = u.User!.Email!,
                    ReservationDate = u.ReservatrionDate,
                    MovieName = u.Movie!.Title!,
                    RoomName = u.Room!.Name,
                    State = u.state
                }).FirstOrDefaultAsync(u => u.Id == reservationId);
                if (movie == null) { throw new Exception("reservation not found"); }
                result.data = movie;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<List<GetReservationDTO>>> GetByUser(string? userID)
        {
            var result = new Response<List<GetReservationDTO>>();
            try
            {
                if(_userManager.FindByIdAsync(userID).Result == null) { throw new Exception("user not found"); }
                var reservations = await _context.Reservations.Include(u => u.Movie).
                    Include(u => u.Room).
                    Include(u => u.User).
                    Where(u=>u.User!.Id== userID).
                    Select(u => new GetReservationDTO()
                {
                    Id = u.Id,
                    UserEmail = u.User!.Email!,
                    ReservationDate = u.ReservatrionDate,
                    MovieName = u.Movie!.Title!,
                    RoomName = u.Room!.Name,
                    State = u.state
                }).ToListAsync();
                result.data = reservations;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<List<GetReservationDTO>>> GetMyReservations()
        {
            var result = new Response<List<GetReservationDTO>>();
            try
            {
                var user = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (_userManager.FindByIdAsync(user) == null) { throw new Exception("user not found"); }
                var reservations = await _context.Reservations.Include(u => u.Movie).
                    Include(u => u.Room).
                    Include(u => u.User).
                    Where(u => u.User!.Id == _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!).
                    Select(u => new GetReservationDTO()
                    {
                        Id = u.Id,
                        UserEmail = u.User!.Email!,
                        ReservationDate = u.ReservatrionDate,
                        MovieName = u.Movie!.Title!,
                        RoomName = u.Room!.Name,
                        State = u.state
                    }).ToListAsync();
                result.data = reservations;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> ToConfirmed(string reservationId)
        {
            var result = new Response<object>();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(u => u.Id == reservationId);
                if (reservation == null) { throw new Exception("reservation not found"); }
                reservation.state=ReservationState.Confirmed;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> ToDenied(string reservationId)
        {
            var result = new Response<object>();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(u => u.Id == reservationId);
                if (reservation == null) { throw new Exception("reservation not found"); }
                reservation.state = ReservationState.Denied;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> ToDone(string reservationId)
        {
            var result = new Response<object>();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(u => u.Id == reservationId);
                if (reservation == null) { throw new Exception("reservation not found"); }
                reservation.state = ReservationState.Done;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> ToWaiting(string reservationId)
        {
            var result = new Response<object>();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(u => u.Id == reservationId);
                if (reservation == null) { throw new Exception("reservation not found"); }
                reservation.state = ReservationState.Waiting;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        
        private bool CheckCollision(Reservation reservation1, Reservation reservation2)
        {
            // Check for overlapping time ranges
            return (reservation1.ReservatrionDateStart <= reservation2.ReservatrionDateStart && reservation1.ReservatrionDateEnd >= reservation2.ReservatrionDateStart)
                || (reservation2.ReservatrionDateStart <= reservation1.ReservatrionDateStart && reservation2.ReservatrionDateEnd >= reservation1.ReservatrionDateStart);
        }
        private Room? GetFirstRoom(List<Room> roomList, Reservation res)
        {
            foreach (Room room in roomList)
            {
                if(Checkroom(room, res)) return room;
            }
            return null;
        }
        private bool Checkroom(Room r, Reservation cr)
        {
            foreach(Reservation res in r.Reservations)
            {
                if (CheckCollision(res, cr)) return false;
            }
            return true;
        }
        private  bool IsBefore10OrAfter2359(TimeOnly t)
        {
            // Check if t is before 10:00
            if (t.CompareTo(TimeOnly.Parse("10:00:00")) < 0)
            {
                return true;
            }

            // Check if t is after 23:59
            if (t.CompareTo(TimeOnly.Parse("23:59:59")) > 0)
            {
                return true;
            }

            return false;
        }
    }
}
