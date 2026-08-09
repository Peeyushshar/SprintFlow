using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintFlow.Infrastructure.Seed
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
}
