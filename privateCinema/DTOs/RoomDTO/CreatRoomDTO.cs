using System.ComponentModel.DataAnnotations;

namespace privateCinema.DTOs.RoomDTO
{
    public class CreatRoomDTO
    {


        public string Name { get; set; } = "";
        [Required]
        [Range(2, 50)]
        public int Capacity { get; set; }
        
    }
}
