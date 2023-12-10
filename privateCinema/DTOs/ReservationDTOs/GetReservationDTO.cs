using Microsoft.AspNetCore.Identity;
using privateCinema.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace privateCinema.DTOs.ReservationDTOs
{
    public class GetReservationDTO
    {
        
        public string? Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public ReservationState State { get; set; } = ReservationState.Waiting;
        
    }
}
