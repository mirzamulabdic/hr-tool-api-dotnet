using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
        public class AuthController : BaseApiController
        {
       
            private readonly UserManager<AppUser> _userManager;
            private readonly ITokenService _tokenService;

            public AuthController(UserManager<AppUser> userManager, ITokenService tokenService)
            {
                this._userManager = userManager;
                this._tokenService = tokenService;
            }


            [HttpPost("login")]
            public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

                if (user == null) return Unauthorized();
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                Console.WriteLine(result);

                if (!result) return Unauthorized();

                return new UserDto
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user),
                };
            }
        }
}
