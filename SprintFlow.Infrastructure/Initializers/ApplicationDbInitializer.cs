using Microsoft.EntityFrameworkCore;
using SprintFlow.Infrastructure.Persistence;
using SprintFlow.Infrastructure.Seed;

namespace SprintFlow.Infrastructure.Initializers
{
    public class ApplicationDbInitializer : IApplicationDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IEnumerable<IDataSeeder> _seeders;

        public ApplicationDbInitializer(
            ApplicationDbContext context,
            IEnumerable<IDataSeeder> seeders
        )
        {
            _context = context;
            _seeders = seeders;
        }

        public async Task InitializeAsync()
        {
            // Apply pending migrations
            await _context.Database.MigrateAsync();

            // Execute all registered seeders
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
