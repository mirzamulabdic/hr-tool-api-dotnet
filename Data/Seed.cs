using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Manager"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "HR"},
                new AppRole{Name = "Employee"},
            };


            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                Email = "admin@hr.com",
                UserName = "admin",
            };

            await userManager.CreateAsync(admin, "P@ssw0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
