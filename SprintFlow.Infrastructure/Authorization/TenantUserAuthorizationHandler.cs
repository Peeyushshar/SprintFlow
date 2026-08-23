using Microsoft.AspNetCore.Authorization;

namespace SprintFlow.Infrastructure.Authorization
{
    public sealed class TenantUserAuthorizationHandler : AuthorizationHandler<TenantUserRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TenantUserRequirement requirement
        )
        {
            // Authentication check
            if (context.User.Identity?.IsAuthenticated != true)
            {
                return Task.CompletedTask;
            }

            // Tenant check
            var tenantId = context.User.FindFirst("tenantId")?.Value;

            if (Guid.TryParse(tenantId, out _))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
