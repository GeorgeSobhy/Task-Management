using Microsoft.AspNetCore.Identity;
using TaskManagement.Shared.Enums;

namespace TaskManagement.API.Seeding
{
    public class SeedingDefaultRoles
    {
        private readonly RoleManager<IdentityRole> roleManger;
        public SeedingDefaultRoles(RoleManager<IdentityRole> roleManger)
        {
            this.roleManger = roleManger;
        }
        public  async Task SeedAsync()
        {
            if (!roleManger.Roles.Any())
            {
                foreach(var role in Enum.GetNames<DefaultRoles>())
                {
                    await roleManger.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
