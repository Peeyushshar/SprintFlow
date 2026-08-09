using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Authentication
{
    public interface IIdentityService
    {
        Task<bool> IsEmailExistsAsync(string email);

        Task<(bool Succeeded, Guid UserId, IEnumerable<string> Errors)> CreateUserAsync(
            ApplicationUser user,
            string password
        );

        Task AddToRoleAsync(Guid userId, string role);

        Task<ApplicationUser?> FindByEmailAsync(string email);
    }
}
