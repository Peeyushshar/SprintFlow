using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace SprintFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //mediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

            // FluentValidation (We'll use this next)
            services.AddValidatorsFromAssembly(assembly);

            // AutoMapper (Later)
            // services.AddAutoMapper(assembly);

            return services;
        }
    }
}
