using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using privateCinema.DTOs.UserDTOs;
using privateCinema.Models;

namespace privateCinema.Services.UsersServices
{
    public class UserServices : IUserServices
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }


        public async Task<Response<List<GetUserDTO>>> GetClientts()
        {
            var result = new Response<List<GetUserDTO>>();
            result.data = new List<GetUserDTO>();
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(Role.Client);
                foreach (var user in users)
                {
                    result.data.Add(new GetUserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = _userManager.GetRolesAsync(user).Result.ToList()[0]
                    });
                }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<List<GetUserDTO>>> GetAll()
        {
            var result = new Response<List<GetUserDTO>>();
            result.data = new List<GetUserDTO>();
            try
            {
                var users = await _userManager.Users.ToListAsync();
                foreach (var user in users)
                {
                    result.data.Add(new GetUserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = _userManager.GetRolesAsync(user).Result.ToList()[0]
                    });
                }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<GetUserDTO>> GetByEmail(string email)
        {
            var result = new Response<GetUserDTO>();
            result.data = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) { throw new Exception("user not found"); }
                {
                    result.data = new GetUserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = _userManager.GetRolesAsync(user).Result.ToList()[0]
                    };
                }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<GetUserDTO>> GetClientByEmail(string email)
        {
            var result = new Response<GetUserDTO>();
            result.data = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null ) { throw new Exception("client not found"); }
                var user_role = await _userManager.GetRolesAsync(user);
                if (user_role[0] != Role.Client ) { throw new Exception("client not found"); }
                {
                    result.data = new GetUserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = _userManager.GetRolesAsync(user).Result.ToList()[0]
                    };
                }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> ChangeRole(AddRoleDTO userole)
        {
            var result = new Response<object>();
            result.data = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(userole.Email);
                if (userole.RoleName == Role.Admin) { throw new Exception("can not make admin"); }
                if (user == null) { throw new Exception("user not found"); }
                if (!_roleManager.RoleExistsAsync(userole.RoleName).Result) { throw new Exception("Role not found"); }
                var currentRole = _userManager.GetRolesAsync(user).Result.ToList()[0];
                if (currentRole == Role.Admin) { throw new Exception("can not change admin role"); }
                var res1 = await _userManager.RemoveFromRoleAsync(user, currentRole);
                if (!res1.Succeeded) { throw new Exception(res1.Errors.FirstOrDefault()!.Description.ToString()); }
                var res2 = await _userManager.AddToRoleAsync(user, userole.RoleName);
                if (!res2.Succeeded) { throw new Exception(res2.Errors.FirstOrDefault()!.Description.ToString()); }

                result.message = "role changed";
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }

        public async Task<Response<object>> DeleteUser(string email)
        {
            var result = new Response<object>();
            result.data = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) { throw new Exception("user not found"); }
                if (_userManager.GetRolesAsync(user).Result.ToList()[0] == Role.Admin) { throw new Exception("can not delete admin"); }
                var res1 = await _userManager.DeleteAsync(user);
                if (!res1.Succeeded) { throw new Exception(res1.Errors.FirstOrDefault()!.Description.ToString()); }
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }



    }


}

