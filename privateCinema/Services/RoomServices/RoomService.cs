using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using privateCinema.DataAccess;
using privateCinema.DTOs.RoomDTO;
using privateCinema.Models;

namespace privateCinema.Services.RoomServices
{
    public class RoomService : IRoomService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        public RoomService(CinemaDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;
        }

        public async Task<Response<object>> CreatRoom(CreatRoomDTO crRoom)
        {
            var result = new Response<object>();
            try
            {
                if (_context.Rooms.FirstOrDefaultAsync(r => r.Name == crRoom.Name).Result != null)
                { throw new Exception("room name already exists"); }
                var room = new Room() { Capacity = crRoom.Capacity, Name = crRoom.Name };
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
                result.message = "Room created successfully";
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<List<GetRoomDTO>>> GetALL()
        {
            var result = new Response<List<GetRoomDTO>>();
            try
            {
                result.data = await _context.Rooms.Select(r => _mapper.Map<GetRoomDTO>(r)).ToListAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<GetRoomDTO>> GetByName(string RoomName)
        {
            var result = new Response<GetRoomDTO>();
            try
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == RoomName);
                if (room == null) { throw new Exception("room name already exists"); }
                result.data = _mapper.Map<GetRoomDTO>(room);
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<object>> DeleteRoom(string RoomName)
        {
            var result = new Response<object>();
            try
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == RoomName);
                if (room == null) { throw new Exception("room does not exists"); }
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                result.message = "room deleted successfully";
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

    }
}
