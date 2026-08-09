using Microsoft.AspNetCore.Mvc;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Enums;

namespace SprintFlow.API.Common.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return result.Error.Type switch
            {
                ErrorType.Validation => new BadRequestObjectResult(result.Error),

                ErrorType.Conflict => new ConflictObjectResult(result.Error),

                ErrorType.NotFound => new NotFoundObjectResult(result.Error),

                ErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error),

                ErrorType.Forbidden => new ObjectResult(result.Error)
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                },

                _ => new BadRequestObjectResult(result.Error),
            };
        }
    }
}
