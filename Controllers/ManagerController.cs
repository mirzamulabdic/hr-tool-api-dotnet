using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ManagerController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IManagerRepository _managerRepository;

        public ManagerController(UserManager<AppUser> userManager, IManagerRepository managerRepository)
        {
            this._userManager = userManager;
            this._managerRepository = managerRepository;
        }

        [Authorize(Policy = "RequireHRManagerRoles")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.JoinedDate,
                        managedByManagerId = (int?)u.Manager.Id,
                        Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                    })
                    .Where(u => u.Id != 1)
                    .AsNoTracking()
                    .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireHRManagerRoles")]
        [HttpGet("list-of-managers")]
        public async Task<ActionResult> GetListOfManagers()
        {

            var managers = await _userManager.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                        .Where(t => t.UserRoles.Any(f => f.Role.Name == "Manager"))
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                    })
                    .AsNoTracking()
                    .ToListAsync();
            return Ok(managers);
        }
    }
}
