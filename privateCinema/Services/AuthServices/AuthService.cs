using Microsoft.AspNetCore.Identity;
using privateCinema.DTOs.LoginDTOs;

namespace privateCinema.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
                if(!created.Succeeded) { throw new Exception(created.Errors.FirstOrDefault().Description.ToString()); }
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
                if (await _userManager.CheckPasswordAsync(user, loguser.Password))
                {
                    result.message = "login successfuly";
                    result.data = "jwt";
                }
                else { throw new Exception("password incorrect"); }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
    }
}
