using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using SprintFlow.API.Common.Exceptions;
using SprintFlow.Application;
using SprintFlow.Infrastructure;
using SprintFlow.Infrastructure.Initializers;

namespace SprintFlow.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            /* builder.Services.AddSwaggerGen()*/;

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SprintFlow", Version = "v1" });

                // JWT Bearer Authentication
                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter JWT token as: Bearer {token}",
                    }
                );

                // Apply Bearer authentication to Swagger endpoints
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            JwtBearerDefaults.AuthenticationScheme,
                            document
                        )
                    ] = [],
                });
            });

            var app = builder.Build();

            // Initialize Database
            using (var scope = app.Services.CreateScope())
            {
                var initializer =
                    scope.ServiceProvider.GetRequiredService<IApplicationDbInitializer>();

                await initializer.InitializeAsync();
            }

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
