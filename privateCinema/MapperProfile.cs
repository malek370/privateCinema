using AutoMapper;
using privateCinema.DTOs.RoomDTO;
using privateCinema.Models;

namespace privateCinema
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Room, GetRoomDTO>();
        }
    }
}
