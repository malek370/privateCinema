using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using privateCinema.DataAccess;
using privateCinema.Services.AuthServices;

namespace privateCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CinemaDbContext>(Options =>
            Options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(Options =>

                Options.Password.RequiredLength = 5
                //i can add more controle here
                ).
                AddEntityFrameworkStores<CinemaDbContext>().
                AddDefaultTokenProviders();

            
            var app = builder.Build();

            
                app.UseSwagger();
                app.UseSwaggerUI();
            
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}