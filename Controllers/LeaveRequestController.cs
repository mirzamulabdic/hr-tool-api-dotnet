using API.Data;
using API.DTOs;
using API.Enums;
using API.Extensions;
using API.Helpers;
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

            var employee = await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());

            var leaveType = newLeaveRequestDto.LeaveType;
            
            bool checkIfLeaveIsPossible;
            checkIfLeaveIsPossible = leaveType switch
            {
                LeaveTypeEnum.Vacation => CheckForVacationDaysBalance(employee.VacationDays, employee.VacationDaysTaken, newLeaveRequestDto.DurationDays),
                LeaveTypeEnum.RemoteWork => CheckForVacationDaysBalance(employee.RemoteWorkDays, employee.RemoteWorkDaysTaken, newLeaveRequestDto.DurationDays),
                LeaveTypeEnum.SickDay => CheckForVacationDaysBalance(employee.SickDays, employee.SickDaysTaken, newLeaveRequestDto.DurationDays),
                _ => CheckForVacationDaysBalance(employee.FamilyDays, employee.FamilyDaysTaken, newLeaveRequestDto.DurationDays),
            };
            
            if (!checkIfLeaveIsPossible) return BadRequest("Leave balance invalid");

            newLeaveRequestDto.UserId = User.GetUserId();
            newLeaveRequestDto.SubmitterFullName = employee.FirstName + " " + employee.LastName;

            if (employee.ManagedBy == null)
            {
                newLeaveRequestDto.ReviewerId = 1;
            }
            else
            {
                newLeaveRequestDto.ReviewerId = employee.ManagedBy ?? default(int);
            }

            _leaveRequestRepository.CreateLeaveRequest(newLeaveRequestDto);
            var leaveBalanceUpdated = await _leaveBalanceRepository.UpdateLeaveBalance(employee.LeaveBalanceId, leaveType, newLeaveRequestDto.DurationDays);

            employee.VacationDaysTaken = leaveBalanceUpdated.VacationDaysTaken;
            employee.RemoteWorkDaysTaken = leaveBalanceUpdated.RemoteWorkDaysTaken;
            employee.FamilyDaysTaken = leaveBalanceUpdated.FamilyDaysTaken;
            employee.SickDaysTaken = leaveBalanceUpdated.SickDaysTaken;


            if (await _leaveRequestRepository.SaveAllAsync()) { 
                return Ok(employee); 
            }

            return BadRequest("Something went wrong");

        }

        [HttpGet("my-leave-requests")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequests([FromQuery] UserParams userParams) 
        {
            userParams.CurrentUserId = User.GetUserId();
            
            var leaveRequests = await _leaveRequestRepository.GetMyLeaveRequestsAsync(userParams);
            
            Response.AddPaginationHeader(new PaginationHeader(leaveRequests.CurrentPage, leaveRequests.PageSize
                , leaveRequests.TotalCount, leaveRequests.TotalPages));

            return Ok(leaveRequests);
        }

        [HttpGet("all-my-leave-requests")]
        public async Task<IEnumerable<LeaveRequestDto>> GetMyAllLeaveRequests()
        {
            return await _leaveRequestRepository.GetAllMyLeaveRequestsWithoutPagiantionAsync(User.GetUserId());
        }

        [Authorize(Policy = "RequireHRManagerRoles")]
        [HttpGet("all-leaves-from-employees")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequestsForMyEmployees([FromQuery] UserParams userParams)
        {
            var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsForMyEmployeesAsync(User.GetUserId(), userParams);

            Response.AddPaginationHeader(new PaginationHeader(leaveRequests.CurrentPage, leaveRequests.PageSize
                , leaveRequests.TotalCount, leaveRequests.TotalPages));

            return Ok(leaveRequests);
        }

        [Authorize(Policy = "RequireHRManagerRoles")]
        [HttpPut("review-leave-request")]
        public async Task<ActionResult> ReviewLeaveRequest(ReviewLeaveRequestDto reviewLeaveRequestDto)
        {

            var employee = await _employeeRepository.GetEmployeeByIdAsync(reviewLeaveRequestDto.EmployeeId);
            Console.WriteLine(reviewLeaveRequestDto.LeaveStatusAction);
            _leaveRequestRepository.ReviewLeaveRequest(reviewLeaveRequestDto.LeaveRequestId, reviewLeaveRequestDto.LeaveStatusAction);

            if (reviewLeaveRequestDto.LeaveStatusAction == LeaveStatusEnum.Rejected)
            {
               var leaveBalanceUpdated = await _leaveBalanceRepository.
                    UpdateLeaveBalance(employee.LeaveBalanceId, reviewLeaveRequestDto.LeaveType, 
                    reviewLeaveRequestDto.DurationDays);
            }
            
            if (await _leaveRequestRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Something went wrong");
        }


        [HttpDelete("cancel/{leaveRequestId}")]
        public async Task<ActionResult> CancelLeaveRequest(int leaveRequestId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());

            if (employee == null) return BadRequest("Bad request");

            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestAsync(leaveRequestId);


            _leaveRequestRepository.CancelLeaveRequest(leaveRequest);
            var leaveBalanceUpdated = await _leaveBalanceRepository.UpdateLeaveBalance(employee.LeaveBalanceId, leaveRequest.LeaveType, -leaveRequest.DurationDays);

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
