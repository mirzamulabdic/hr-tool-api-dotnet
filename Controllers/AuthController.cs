using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
        public class AuthController : BaseApiController
        {
            private readonly DataContext _dataContext;
            private readonly ITokenService _tokenService;

            public AuthController(DataContext dataContext, ITokenService tokenService)
            {
                this._dataContext = dataContext;
                this._tokenService = tokenService;
            }

            [HttpPost("register")]
            public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
            {
                if (await UserExists(registerDto.Email)) return BadRequest("That user already exist");

                var user = new AppUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    BirthDate = DateTime.UtcNow,
                    City = registerDto.City,
                    Street = registerDto.Street,
                    Gender = registerDto.Gender,
                    PhoneNumber = "061",
                    JoinedDate = new DateOnly(),
                    LeaveBalance = new LeaveBalance()
                };

                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();

                return new UserDto
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email,
                    Token = "test"
                };
            }

            [HttpPost("login")]
            public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
                var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

                if (user == null) return BadRequest();

                if (user.Password != loginDto.Password) return BadRequest("Unauthorized");

                return new UserDto
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                };
            }

            public async Task<bool> UserExists(string email)
            {
                return await _dataContext.Users.AnyAsync(x => x.Email == email.ToLower());
            }
        }
}
