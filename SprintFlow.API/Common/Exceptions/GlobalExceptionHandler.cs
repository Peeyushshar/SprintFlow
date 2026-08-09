using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace SprintFlow.API.Common.Exceptions
{
    public sealed class GlobalExceptionHandler
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred.");

            var response = new
            {
                Success = false,
                Error = exception switch
                {
                    AppException ex => new
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message
                    },

                    _ => new
                    {
                        Code = "ServerError",
                        Message = "An unexpected error occurred."
                    }
                }
            };

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception switch
            {
                AppException ex => ex.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                cancellationToken);

            return true;
        }
    }
}
