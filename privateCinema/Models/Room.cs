using System.ComponentModel.DataAnnotations;

namespace privateCinema.Models
{
    public class Room
    {
        public string Id { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }= DateTime.Now;
        [Required]
        public string Name { get; set; } = "";
        [Required]
        [Range(2,50)]
        public int Capacity { get; set; }
        [Required]
        public bool Clean { get; set; } = true;


    }
}
