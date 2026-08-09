using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            return await _context.Tenants.AnyAsync(t => t.Slug == slug);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Tenants.AnyAsync(t => t.Name == name);
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant?> GetBySlugAsync(string slug)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug);
        }

        public void Update(Tenant entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Tenant entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tenant> Query()
        {
            throw new NotImplementedException();
        }
    }
}
