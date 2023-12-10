using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using privateCinema.DTOs.ReservationDTOs;
using privateCinema.Models;
using privateCinema.Services.ReservationService;
using privateCinema.Services;

namespace privateCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IResertvationService _ReservationService;
        public ReservationController(IResertvationService ReservationService)
        {
            _ReservationService = ReservationService;
        }


        [HttpPost("MakeReservation")]
        [Authorize(Roles = Role.Client)]
        public async Task<IActionResult> MakeReservation(CreatReservationDTO reservation)
        {
            var res = await _ReservationService.Creat(reservation);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }


        [HttpPost("DeleteReservation")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> DeleteReservation(string reservationId)
        {
            var res = await _ReservationService.Delete(reservationId);
            if (res.success) return Ok(res);
            return BadRequest(res);
        }


        [HttpPost("ToWaiting")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> ChangeStateReservation(ChangeStateDTO change)
        {
            Services.Response<object> res;
            switch (change.action)
            {
                case 1: res = await _ReservationService.ToWaiting(change.ReservationId); break;
                case 2: res = await _ReservationService.ToDenied(change.ReservationId); break;
                case 3: res = await _ReservationService.ToDone(change.ReservationId); break;
                case 4: res = await _ReservationService.ToConfirmed(change.ReservationId); break;
                default:
                    res = new Services.Response<object>() { success = false, message = "invalid action" };
                    break;
            }
            if (res.success) return Ok(res);
            return BadRequest(res);
        }


        [HttpPost("MyReservations")]
        [Authorize(Roles = Role.Client)]
        public async Task<IActionResult> MyReservations()
        {
            var res = await _ReservationService.GetMyReservations();
            if (res.success) return Ok(res);
            return BadRequest(res);
        }
    }c
}
