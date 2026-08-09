using Microsoft.AspNetCore.Identity;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Seed
{
    public sealed class AdminSeeder : IDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            const string email = "admin@sprintflow.com";
            const string password = "Admin@123";

            var existing = await _userManager.FindByEmailAsync(email);

            if (existing is not null)
                return;

            var admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Platform",
                LastName = "Admin",
                TenantId = null,
            };

            var result = await _userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => $"{e.Code}: {e.Description}")
                );

                throw new InvalidOperationException($"Failed to create platform admin: {errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(admin, Roles.Admin);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(e => $"{e.Code}: {e.Description}")
                );

                throw new InvalidOperationException($"Failed to assign Admin role: {errors}");
            }
        }
    }
}
