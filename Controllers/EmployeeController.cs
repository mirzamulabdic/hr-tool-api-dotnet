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

    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IEmailService _emailService;

        public EmployeeController(UserManager<AppUser> userManager,
            DataContext dataContext,
            IEmployeeRepository employeeRepository, 
            ILeaveBalanceRepository leaveBalanceRepository,
            IEmailService emailService)
        {
            this._userManager = userManager;
            this._dataContext = dataContext;
            this._employeeRepository = employeeRepository;
            this._leaveBalanceRepository = leaveBalanceRepository;
            this._emailService = emailService;
        }
        [Authorize]
        [HttpGet("my-info")]
        public async Task<ActionResult<EmployeeDto>> GetMyInfo()
        {
            return await _employeeRepository.GetEmployeeByIdAsync(User.GetUserId());
        }

        [Authorize]
        [Authorize(Policy = "RequireHRManagerRoles")]
        [HttpPost("new-employee")]
        public async Task<ActionResult<UserDto>> AddNewEmployee(NewEmployeeDto newEmployeeDto)
        {
            if (await UserExists(newEmployeeDto.Email)) return BadRequest("That user already exist");


            var user = new AppUser
            {
                FirstName = newEmployeeDto.FirstName,
                LastName = newEmployeeDto.LastName,
                UserName = (newEmployeeDto.FirstName + newEmployeeDto.LastName).ToLower(),
                Email = newEmployeeDto.Email,
                BirthDate = newEmployeeDto.BirthDate,
                City = newEmployeeDto.City,
                Street = newEmployeeDto.Street,
                Gender = newEmployeeDto.Gender,
                PhoneNumber = newEmployeeDto.PhoneNumber,
                JoinedDate = DateOnly.FromDateTime(newEmployeeDto.JoinedDate),
                LeaveBalance = new LeaveBalance(),
                
            };

            var result = await _userManager.CreateAsync(user, (newEmployeeDto.FirstName + newEmployeeDto.LastName).ToLower() + "HR1!");

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var cofirmationLink = "<h2>Confirm your email by clicking on this link: </h2><br><br>" +
                  Url.Action(nameof(ConfirmEmail), "Employee", new {token, email = user.Email}, Request.Scheme) + "<br><br><a>HR System Automated Email</a>";

            var message = new EmailMessageDto
            {
                To = user.Email,
                Subject = "Confirmation email link",
                Content = cofirmationLink!,
            };

            _emailService.SendEmail(message);

            var managing = new UserManage
            {
                ManagerId = newEmployeeDto.ManagerId,
                Employee = user
            };

            _dataContext.UserManages.Add(managing);


            await _dataContext.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.Id.ToString(),
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet("leave-balance")]
        public async Task<ActionResult<LeaveBalanceDto>> GetLeaveBalance() 
        {
            return await _employeeRepository.GetLeaveBalance(User.GetUserId());
        }

        [Authorize]
        [HttpPut("update-leave-balance")]
        public async Task<ActionResult> UpdateLeaveBalance(LeaveBalanceUpdateDto leaveBalanceUpdateDto)
        {
            var updatedLeaveBalance = _leaveBalanceRepository.UpdateLeaveBalance(leaveBalanceUpdateDto.LeaveBalanceId, leaveBalanceUpdateDto.LeaveType ,leaveBalanceUpdateDto.Days);
            return Ok();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) return BadRequest("Email confirmation failed!");

            return Ok("You have successfully confirmed your Email Adress");
        }

        public async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
