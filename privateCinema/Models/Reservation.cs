using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace privateCinema.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [Required]
        public IdentityUser? User { get; set; }
        [Required]
        public DateTime ReservatrionDate { get; set; }
        [Required]
        public Movie? Movie { get; set; }
        public Room? Room { get; set; }
        public ReservationState state { get; set; }=ReservationState.Waiting;
        public DateTime ReservatrionDateStart { get; set; }
        public DateTime ReservatrionDateEnd { get; set; }


    }
}
