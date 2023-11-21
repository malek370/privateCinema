using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using privateCinema.DTOs.LoginDTOs;
using privateCinema.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace privateCinema.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        
        public async Task<Response<object>> Register(RegisterDTO reguser)
        {
            var result = new Response<object>();
            try
            {
                if(await _userManager.FindByEmailAsync(reguser.Email)!=null) { throw new Exception("user exists"); }
                var user = new IdentityUser()
                {
                    UserName = reguser.Username,
                    Email = reguser.Email,
                    PhoneNumber = reguser.PhoneNumber,
                };
                var created=await _userManager.CreateAsync(user, reguser.Password);
                await _userManager.AddToRoleAsync(user,Role.Client);
                if(!created.Succeeded) { throw new Exception(created.Errors.FirstOrDefault()!.Description.ToString()); }
                result.message = "registredf successfully";
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<string>> Login(LoginDTO loguser)
        {
            var result = new Response<string>();
            try
            {
                var user = await _userManager.FindByEmailAsync(loguser.Email);
                if (user == null) { throw new Exception("user not found"); }
                if (!await _userManager.CheckPasswordAsync(user, loguser.Password))throw new Exception("password incorrect");
                result.message = "login successfuly";
                result.data = await GenerateToken(user);
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]!));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
