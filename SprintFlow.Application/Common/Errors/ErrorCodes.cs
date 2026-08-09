namespace SprintFlow.Application.Common.Errors
{
    public static class ErrorCodes
    {
        //Register
        public const string TenantAlreadyExists = "TENANT_001";
        public const string EmailAlreadyExists = "AUTH_001";
        public const string RoleNotFound = "AUTH_002";
        public const string UserCreationFailed = "AUTH_003";
        public const string Unexpected = "SYS_500";
        //Login
        public const string InvalidCredentials = "AUTH_004";
        public const string InactiveUser = "AUTH_005";
        //RefreshToken
        public const string InvalidRefreshToken = "AUTH_006";
        public const string RefreshTokenRevoked = "AUTH_007";
        public const string RefreshTokenExpired = "AUTH_008";
    }
}
