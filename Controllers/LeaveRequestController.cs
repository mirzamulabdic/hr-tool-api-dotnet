using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LeaveRequestController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestController(DataContext dataContext, IEmployeeRepository employeeRepository, ILeaveRequestRepository leaveRequestRepository)
        {
            this._dataContext = dataContext;
            this._employeeRepository = employeeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
        }

        [Authorize]
        [HttpPost("new-request")]
        public async Task<ActionResult<EmployeeDto>> CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto)
        {
            // Vacation RemoteWork SickDay FamilyLeave

            var employee = await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());

            string leaveType = newLeaveRequestDto.LeaveType;

            if (leaveType != "Vacation" && leaveType != "RemoteWork" && leaveType != "SickDay" && leaveType != "FamilyLeave") 
            {
                return BadRequest("Invalid leave type!");
            }

            bool checkIfLeaveIsPossible;
            checkIfLeaveIsPossible = leaveType switch
            {
                "Vacation" => CheckForVacationDaysBalance(employee.VacationDays, employee.VacationDaysTaken, newLeaveRequestDto.DurationDays),
                "RemoteWork" => CheckForVacationDaysBalance(employee.RemoteWorkDays, employee.RemoteWorkDaysTaken, newLeaveRequestDto.DurationDays),
                "SickDay" => CheckForVacationDaysBalance(employee.SickDays, employee.SickDaysTaken, newLeaveRequestDto.DurationDays),
                _ => CheckForVacationDaysBalance(employee.FamilyDays, employee.FamilyDaysTaken, newLeaveRequestDto.DurationDays),
            };

            if (!checkIfLeaveIsPossible) return BadRequest("Something went wrong");

            newLeaveRequestDto.UserId = User.GetUserId();

            _leaveRequestRepository.CreateLeaveRequest(newLeaveRequestDto);

            if (await _leaveRequestRepository.SaveAllAsync()) return Ok();

            return BadRequest("Something went wrong");

        }

        public bool CheckForVacationDaysBalance(int leaveTypeBalance, int leaveTypeTakenDays, int leaveDuration)
        {
            if ((leaveTypeBalance == leaveTypeTakenDays) || (leaveTypeBalance - leaveTypeTakenDays) < leaveDuration) return false;

            if ((leaveTypeBalance - leaveTypeTakenDays) < leaveDuration) return false;

            return true;
            
        }


    } 
}
