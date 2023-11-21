using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using privateCinema.DataAccess;
using privateCinema.Services.AuthServices;
using privateCinema.Services.RoomServices;
using privateCinema.Services.UsersServices;
using System.Text;

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
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IRoomService,RoomService>();

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

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]!))
                };
            });
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