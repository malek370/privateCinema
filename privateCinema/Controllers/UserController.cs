using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using privateCinema.DTOs.UserDTOs;
using privateCinema.Services.UsersServices;

namespace privateCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("allUsers")]
        public async Task<IActionResult> Get()
        {
            var res = await _userServices.GetAll();
            if(res.success) return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("GetByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmail(string email)
        {
           var res=await _userServices.GetByEmail(email);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("changeRole")]
        public async Task<IActionResult> changeRole(AddRoleDTO userole)
        {
            var res = await _userServices.ChangeRole(userole);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser(string email)
        {
            var res = await _userServices.DeleteUser(email);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("client")]
        public async Task<IActionResult> GetClientByEmail(string email)
        {
            var res = await _userServices.GetClientByEmail(email);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("allClients")]
        public async Task<IActionResult> GetClients()
        {
            var res = await _userServices.GetClientts();
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

    }
}
