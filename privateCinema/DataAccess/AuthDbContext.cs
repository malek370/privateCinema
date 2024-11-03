using Ath.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ath.DataAccess
{
    public class AuthDbContext : IdentityDbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
