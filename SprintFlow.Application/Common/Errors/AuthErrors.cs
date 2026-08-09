using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Enums;

namespace SprintFlow.Application.Common.Errors
{
    public static class AuthErrors
    {
        public static readonly Error EmailAlreadyExists = new(
            "AUTH_001",
            "Email already exists.",
            ErrorType.Conflict
        );

        public static readonly Error OwnerRoleMissing = new(
            ErrorCodes.RoleNotFound,
            "Owner role not found.",
            ErrorType.NotFound
        );

        public static readonly Error UserCreationFailed = new(
            ErrorCodes.UserCreationFailed,
            "Unable to create user.",
            ErrorType.Failure
        );

        public static readonly Error InvalidCredentials = new(
            ErrorCodes.InvalidCredentials,
            "Invalid Credentials.",
            ErrorType.NotFound
        );
        public static readonly Error InactiveUser = new(
            ErrorCodes.InactiveUser,
            "User is inactive",
            ErrorType.Forbidden
        );
        public static readonly Error InvalidRefreshToken = new(
            ErrorCodes.InvalidRefreshToken,
            "Refresh token is invalid!",
            ErrorType.NotFound
        );
        public static readonly Error RefreshTokenRevoked = new(
            ErrorCodes.RefreshTokenRevoked,
            "Refresh token is revoked.",
            ErrorType.Validation
        );
        public static readonly Error RefreshTokenExpired = new(
            ErrorCodes.RefreshTokenExpired,
            "Refresh token is expired.",
            ErrorType.Validation
        );
    }
}
