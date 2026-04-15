using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Entities.Identity;
using TaskManagement.Shared.Enums;

namespace TaskManagement.API.Seeding
{
    public class SeedingDefaultUsers
    {
        private readonly UserManager<ApplicationUser> userManager;
        public SeedingDefaultUsers(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task SeedBasicUserAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "basicuser@example.com",
                Email = "basicuser@example.com",
                EmailConfirmed = true
                
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "QWEqwe123!@#");
                await userManager.AddToRoleAsync(defaultUser, DefaultRoles.NewUser.ToString());
            }
        }

        public async Task SeedSuperAdminUserAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email.ToLower());

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                user = defaultUser;
            }

            if (!await userManager.IsInRoleAsync(user, DefaultRoles.Admin.ToString()))
            {
                await userManager.AddToRoleAsync(user, DefaultRoles.Admin.ToString());
            }
        }
    }
}
