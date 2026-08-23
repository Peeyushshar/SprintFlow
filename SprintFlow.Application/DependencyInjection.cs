using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SprintFlow.Application.Common.Behaviors;

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

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });

            // FluentValidation (We'll use this next)
            services.AddValidatorsFromAssembly(assembly);

            // AutoMapper (Later)
            services.AddAutoMapper(cfg => { }, assembly);

            return services;
        }
    }
}
