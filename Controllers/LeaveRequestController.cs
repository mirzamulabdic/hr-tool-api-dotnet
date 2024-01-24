using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LeaveRequestController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public LeaveRequestController(DataContext dataContext, IEmployeeRepository employeeRepository, 
            ILeaveRequestRepository leaveRequestRepository, 
            ILeaveBalanceRepository leaveBalanceRepository)
        {
            this._dataContext = dataContext;
            this._employeeRepository = employeeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveBalanceRepository = leaveBalanceRepository;
        }

        [HttpPost("new-request")]
        public async Task<ActionResult<EmployeeDto>> CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto)
        {
            // Vacation RemoteWork SickDay FamilyLeave

            var employee = await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());

            string leaveType = newLeaveRequestDto.LeaveType;

            if (leaveType != "vacation" && leaveType != "remotework" && leaveType != "sickday" && leaveType != "familyleave")
            {
                return BadRequest("Invalid leave type!");
            }

            bool checkIfLeaveIsPossible;
            checkIfLeaveIsPossible = leaveType switch
            {
                "vacation" => CheckForVacationDaysBalance(employee.VacationDays, employee.VacationDaysTaken, newLeaveRequestDto.DurationDays),
                "remotework" => CheckForVacationDaysBalance(employee.RemoteWorkDays, employee.RemoteWorkDaysTaken, newLeaveRequestDto.DurationDays),
                "sickday" => CheckForVacationDaysBalance(employee.SickDays, employee.SickDaysTaken, newLeaveRequestDto.DurationDays),
                _ => CheckForVacationDaysBalance(employee.FamilyDays, employee.FamilyDaysTaken, newLeaveRequestDto.DurationDays),
            };

            if (!checkIfLeaveIsPossible) return BadRequest("Leave balance invalid");

            newLeaveRequestDto.UserId = User.GetUserId();

            _leaveRequestRepository.CreateLeaveRequest(newLeaveRequestDto);

            _leaveBalanceRepository.UpdateLeaveBalance(employee.LeaveBalanceId, leaveType, newLeaveRequestDto.DurationDays);
            if (await _leaveRequestRepository.SaveAllAsync()) return Ok(employee);

            return BadRequest("Something went wrong");

        }

        [HttpGet]
        public async Task<IEnumerable<LeaveRequestDto>> GetLeaveRequests() 
        {
            return await _leaveRequestRepository.GetLeaveRequestsAsync(User.GetUserId());
        }

        [HttpDelete("cancel/{leaveRequestId}")]
        public async Task<ActionResult> CancelLeaveRequest(int leaveRequestId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());

            if (employee == null) return BadRequest("Bad request");

            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestAsync(leaveRequestId);

            //Console.WriteLine(leaveRequest.DurationDays);
            _leaveRequestRepository.CancelLeaveRequest(leaveRequest);
            _leaveBalanceRepository.UpdateLeaveBalance(employee.LeaveBalanceId, leaveRequest.LeaveType, -leaveRequest.DurationDays);

            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        public bool CheckForVacationDaysBalance(int leaveTypeBalance, int leaveTypeTakenDays, int leaveDuration)
        {
            if ((leaveTypeBalance == leaveTypeTakenDays) || (leaveTypeBalance - leaveTypeTakenDays) < leaveDuration) return false;

            if ((leaveTypeBalance - leaveTypeTakenDays) < leaveDuration) return false;

            return true;
            
        }


    } 
}
