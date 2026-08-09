using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintFlow.Infrastructure.Initializers
{
    public interface IApplicationDbInitializer
    {
        Task InitializeAsync();
    }
}
