using Microsoft.AspNetCore.Identity;
using SprintFlow.Domain.Constants;

namespace SprintFlow.Infrastructure.Seed
{
    public class RoleSeeder : IDataSeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            foreach (var role in Roles.All)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                }
            }
        }
    }
}
