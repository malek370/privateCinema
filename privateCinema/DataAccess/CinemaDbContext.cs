using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using privateCinema.Models;

namespace privateCinema.DataAccess
{
    public class CinemaDbContext:IdentityDbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public CinemaDbContext(DbContextOptions options) : base(options) 
        {
            
        }
    }
}
