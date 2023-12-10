using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using privateCinema.DTOs.RoomDTO;
using privateCinema.Services.RoomServices;

namespace privateCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("creat")]
        public async Task<IActionResult> creat(CreatRoomDTO creatRoomDTO)
        {
            var res = await _roomService.CreatRoom(creatRoomDTO);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _roomService.GetALL();
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("ByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var res = await _roomService.GetByName(name);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletRoom")]
        public async Task<IActionResult> delete(string name)
        {
            var res = await _roomService.DeleteRoom(name);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpPut("Clean")]
        public async Task<IActionResult> Clean(string roomname)
        {
            var res = await _roomService.CleanRoom(roomname);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
    }
}
