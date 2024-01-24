using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, ILeaveBalanceRepository leaveBalanceRepository)
        {
            this._employeeRepository = employeeRepository;
            this._leaveBalanceRepository = leaveBalanceRepository;
        }

        [HttpGet("leave-balance")]
        public async Task<ActionResult<LeaveBalanceDto>> GetLeaveBalance() 
        {
            return await _employeeRepository.GetLeaveBalance(User.GetUserId());
        }

        [HttpPut("update-leave-balance")]
        public async Task<ActionResult> UpdateLeaveBalance(LeaveBalanceUpdateDto leaveBalanceUpdateDto)
        {
            _leaveBalanceRepository.UpdateLeaveBalance(leaveBalanceUpdateDto.LeaveBalanceId,leaveBalanceUpdateDto.LeaveType ,leaveBalanceUpdateDto.Days);
            return Ok();
        }

    }
}
