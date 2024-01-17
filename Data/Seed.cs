using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;

            var testUser = new AppUser
            {
                FirstName = "Test",
                LastName = "Test",
                Password = "Test",
                Email = "test@test.com",
                Gender = "test",
                City = "Test",
                Street = "Test",
                BirthDate = DateTime.Now
            };

            dataContext.Users.Add(testUser);
            await dataContext.SaveChangesAsync();
        }
    }
}
