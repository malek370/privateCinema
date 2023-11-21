using privateCinema.DTOs.RoomDTO;

namespace privateCinema.Services.RoomServices
{
    public interface IRoomService
    {
        Task<Response<object>> CreatRoom(CreatRoomDTO crRoom);
        Task<Response<object>> DeleteRoom(string RoomName);
        Task<Response<List<GetRoomDTO>>> GetALL();
        Task<Response<GetRoomDTO>> GetByName(string RoomName);
    }
}