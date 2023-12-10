using privateCinema.DTOs.MovieDTO;
using privateCinema.DTOs.ReservationDTOs;

namespace privateCinema.Services.ReservationService
{
    public interface IResertvationService
    {
        Task<Response<GetReservationDTO>> Creat(CreatReservationDTO creatreservation);
        Task<Response<object>> Delete(string reservationId);
        Task<Response<GetReservationDTO>> GetByID(string reservationId);
        Task<Response<List<GetReservationDTO>>> GetByUser(string? userID);
        Task<Response<List<GetReservationDTO>>> GetAll();
        Task<Response<object>> ToWaiting(string reservationId);
        Task<Response<object>> ToConfirmed(string reservationId);
        Task<Response<object>> ToDenied(string reservationId);
        Task<Response<object>> ToDone(string reservationId);
        Task<Response<List<GetReservationDTO>>> GetMyReservations();

    }
}
