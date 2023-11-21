using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace privateCinema.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
