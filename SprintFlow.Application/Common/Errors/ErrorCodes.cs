namespace SprintFlow.Application.Common.Errors
{
    public static class ErrorCodes
    {
        //Common
        public const string NotFound = "SPRINT_FLOW_001";

        //Tenant
        public const string TenantAlreadyExists = "TENANT_001";
        public const string EmailAlreadyExists = "AUTH_001";
        public const string OwnerEmailAlreadyExists = "TENANT_002";
        public const string RoleNotFoundCode = "AUTH_002";
        public const string RoleNotFoundMessage = "Role not found.";
        public const string RoleRequiredCode = "AUTH_003";
        public const string RoleRequiredMessage = "Roles are required.";
        public const string RoleAssignmentFailedCode = "AUTH_004";
        public const string RoleAssignmentFailedMessage = "Role assignment failed.";
        public const string UserCreationFailed = "AUTH_005";
        public const string Unexpected = "SYS_500";
        public const string PhoneNumberAssignmentFailedCode = "AUTH_006";
        public const string PhoneNumberAssignmentFailedMessage =
            "Phone number assignment failed, please check your phone number.";
        public const string UserUpdateFailedCode = "AUTH_007";
        public const string UserUpdateFailedMessage =
            "User updation failed due to some reason, try again later!.";

        //Login
        public const string InvalidCredentials = "AUTH_004";
        public const string InactiveUser = "AUTH_005";

        //RefreshToken
        public const string InvalidRefreshToken = "AUTH_006";
        public const string RefreshTokenRevoked = "AUTH_007";
        public const string RefreshTokenExpired = "AUTH_008";

        //Project
        public const string TenantIdRequiredCode = "PROJECT_001";
        public const string TenantIdRequiredMessage = "TenantId is required!";
        public const string ProjectKeyAlreadyExistsCode = "PROJECT_002";
        public const string ProjectKeyAlreadyExistsMessage = "ProjectKey is already present.";
        public const string ProjectNotFoundCode = "PROJECT_003";
        public const string ProjectNotFoundMessage =
            "Project not found with the current project id";
    }
}
