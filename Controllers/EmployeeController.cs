using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public EmployeeController(UserManager<AppUser> userManager,
            IEmployeeRepository employeeRepository, 
            ILeaveBalanceRepository leaveBalanceRepository)
        {
            this._userManager = userManager;
            this._employeeRepository = employeeRepository;
            this._leaveBalanceRepository = leaveBalanceRepository;
        }

        [HttpPost("new-employee")]
        public async Task<ActionResult<UserDto>> AddNewEmployee(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email)) return BadRequest("That user already exist");


            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = (registerDto.FirstName + registerDto.LastName).ToLower(),
                Email = registerDto.Email,
                BirthDate = registerDto.BirthDate,
                City = registerDto.City,
                Street = registerDto.Street,
                Gender = registerDto.Gender.ToLower(),
                PhoneNumber = registerDto.PhoneNumber,
                JoinedDate = DateOnly.FromDateTime(registerDto.JoinedDate),
                LeaveBalance = new LeaveBalance()
            };

            var result = await _userManager.CreateAsync(user, (registerDto.FirstName + registerDto.LastName).ToLower() + "HR1!");

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                UserId = user.Id.ToString(),
                Email = user.Email
            };
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

        public async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
