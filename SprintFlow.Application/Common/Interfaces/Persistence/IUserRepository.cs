using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<ApplicationUser?> GetByEmailAsync(string email);
    }
}
