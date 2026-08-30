using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Common.Errors
{
    public static class TenantErrors
    {
        public static readonly Error SlugAlreadyExists = new(
            ErrorCodes.TenantAlreadyExists,
            "Company slug already exists.",
            Enums.ErrorType.Conflict
        );

        public static readonly Error OwnerEmailAlreadyExists = new(
            ErrorCodes.OwnerEmailAlreadyExists,
            "Owner email already exists.",
            Enums.ErrorType.Conflict
        );
    }
}
