using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Enums;

namespace SprintFlow.Application.Common.Errors
{
    public static class ProjectErrors
    {
        public static readonly Error TenantRequired = new(
            ErrorCodes.TenantIdRequiredCode,
            ErrorCodes.TenantIdRequiredMessage,
            ErrorType.Failure
        );

        public static readonly Error ProjectKeyAlreadyExists = new(
            ErrorCodes.ProjectKeyAlreadyExistsCode,
            ErrorCodes.ProjectKeyAlreadyExistsMessage,
            ErrorType.Conflict
        );

        public static readonly Error ProjectNotFound = new(
            ErrorCodes.ProjectNotFoundCode,
            ErrorCodes.ProjectNotFoundMessage,
            ErrorType.Unauthorized
        );
    }
}
