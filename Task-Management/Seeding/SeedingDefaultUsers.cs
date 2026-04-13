using Microsoft.AspNetCore.Identity;
using TaskManagement.Shared.Enums;

namespace TaskManagement.API.Seeding
{
    public class SeedingDefaultUsers
    {
        private readonly UserManager<IdentityUser> userManager;
        public SeedingDefaultUsers(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task SeedBasicUserAsync()
        {
            var defaultUser = new IdentityUser
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
            var defaultUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
                ,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email.ToLower());

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, DefaultRoles.NewUser.ToString());

            }

        }
    }
}
