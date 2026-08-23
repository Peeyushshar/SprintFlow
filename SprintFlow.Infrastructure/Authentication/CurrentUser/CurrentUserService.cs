using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SprintFlow.Application.Common.Interfaces.Authentication;

namespace SprintFlow.Infrastructure.Authentication.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

                return Guid.TryParse(userId, out var id) ? id : null;
            }
        }

        public Guid? TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("tenantId");

                return Guid.TryParse(tenantId, out var id) ? id : null;
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
