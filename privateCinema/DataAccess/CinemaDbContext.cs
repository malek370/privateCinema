using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using privateCinema.Models;

namespace privateCinema.DataAccess
{
    public class CinemaDbContext:IdentityDbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public CinemaDbContext(DbContextOptions options) : base(options) 
        {
            
        }
    }
}
