using Microsoft.AspNetCore.Identity;
using privateCinema.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace privateCinema.DTOs.ReservationDTOs
{
    public class CreatReservationDTO
    {
        
        
        [Required]
        public int MovieId { get; set; }
        public int NbInvited { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
    }
}
