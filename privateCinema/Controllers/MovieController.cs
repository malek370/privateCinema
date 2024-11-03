using Ath.DTOs.MovieDTO;
using Ath.Services.MovieServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ath.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService roomService)
        {
            _movieService = roomService;
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("creat")]
        public async Task<IActionResult> Creat(CreatMovieDTO creatMovie)
        {
            var res = await _movieService.Creat(creatMovie);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

 
        [HttpGet("byTitle")]
        public async Task<IActionResult> GetTitle(string? title)
        {
            var res = await _movieService.GetByTitle(title);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

        [Authorize]
        [HttpGet("Bykeywords")]
        public async Task<IActionResult> GetByKeywords(string keywords)
        {
            var res = await _movieService.GetByKeywords(keywords);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> delete(int id)
        {
            var res = await _movieService.Delete(id);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Id")]
        public async Task<IActionResult> getId(int id)
        {
            var res = await _movieService.GetByID(id);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
    }

}
