using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LeaveEventsController : BaseApiController
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveEventsController(ILeaveRequestRepository leaveRequestRepository)
        {
            this._leaveRequestRepository = leaveRequestRepository;
        }

        [HttpGet("employees-on-leave-today")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetEmployeesOnLeaveToday()
        {
            //start: 05.Februar.2024  end: 10.Frebruar.2024

            //08.Februar.2024
            //Where()
            var leaveEvents = await _leaveRequestRepository.GetLeaveEventsToday();


            return Ok(leaveEvents);
        }
    }
}
