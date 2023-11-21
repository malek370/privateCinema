using System.ComponentModel.DataAnnotations;

namespace privateCinema.DTOs.RoomDTO
{
    public class GetRoomDTO
    {
        public DateTime CreationDate { get; set; }
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public bool Clean { get; set; } 
    }
}
