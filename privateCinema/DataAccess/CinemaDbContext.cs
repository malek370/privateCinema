using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace privateCinema.DataAccess
{
    public class CinemaDbContext:IdentityDbContext
    {
        public CinemaDbContext(DbContextOptions options) : base(options) 
        {
            
        }
    }
}
