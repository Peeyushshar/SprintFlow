using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.API.Common.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment
    )
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (httpContext.Response.HasStarted)
        {
            _logger.LogWarning(
                exception,
                "Cannot handle exception because the response has already started."
            );

            return false;
        }

        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            httpContext.TraceIdentifier
        );

        var (statusCode, error) = exception switch
        {
            // FluentValidation
            ValidationException ex => (
                StatusCodes.Status400BadRequest,
                new ErrorResponse(
                    "ValidationError",
                    string.Join(" ", ex.Errors.Select(e => e.ErrorMessage))
                )
            ),

            // Your application/business exceptions
            AppException ex => (
                StatusCodes.Status400BadRequest,
                new ErrorResponse(ex.ErrorCode, ex.Message)
            ),

            // Authentication
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                new ErrorResponse("Unauthorized", "You are not authorized to perform this action.")
            ),

            // Resource not found
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                new ErrorResponse("NotFound", "The requested resource was not found.")
            ),

            // AutoMapper
            AutoMapperMappingException ex => (
                StatusCodes.Status500InternalServerError,
                CreateMappingError(ex)
            ),

            // Request cancelled
            OperationCanceledException when cancellationToken.IsCancellationRequested => (
                StatusCodes.Status499ClientClosedRequest,
                new ErrorResponse("RequestCancelled", "The request was cancelled.")
            ),

            // Everything else
            _ => (StatusCodes.Status500InternalServerError, CreateServerError(exception)),
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new
        {
            Success = false,
            Error = error,
            TraceId = httpContext.TraceIdentifier,
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private ErrorResponse CreateMappingError(AutoMapperMappingException exception)
    {
        if (_environment.IsDevelopment())
        {
            return new ErrorResponse("MappingError", exception.Message);
        }

        return new ErrorResponse(
            "MappingError",
            "An error occurred while processing the requested data."
        );
    }

    private ErrorResponse CreateServerError(Exception exception)
    {
        if (_environment.IsDevelopment())
        {
            return new ErrorResponse(exception.GetType().Name, exception.Message);
        }

        return new ErrorResponse("ServerError", "An unexpected error occurred.");
    }
}
