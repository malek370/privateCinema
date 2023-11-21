using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using privateCinema.DTOs.LoginDTOs;
using privateCinema.Services;
using privateCinema.Services.AuthServices;

namespace privateCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
     {
        private readonly IAuthService _authService;
        public AuthentificationController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")] 
        public async Task<IActionResult> Login(LoginDTO loguser)
        {
            var res=await _authService.Login(loguser);
            if(res.success) { return Ok(res); }
            return BadRequest(res);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO reguser)
        {
            var res = await _authService.Register(reguser);
            if (res.success) { return Ok(res); }
            return BadRequest(res);
        }
        
    }
}
