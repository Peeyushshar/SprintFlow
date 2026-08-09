using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
