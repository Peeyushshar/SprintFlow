using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Enums;

namespace SprintFlow.Application.Common.Errors
{
    public static class CommonErrors
    {
        public static readonly Error EntityNotFound = new(
            ErrorCodes.NotFound,
            "Entity does not exists.",
            ErrorType.NotFound
        );

        public static readonly Error TenantRequired = new(
            ErrorCodes.TenantIdRequiredCode,
            ErrorCodes.TenantIdRequiredMessage,
            ErrorType.Failure
        );
    }
}
