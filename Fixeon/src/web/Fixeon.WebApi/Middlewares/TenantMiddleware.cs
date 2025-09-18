using Fixeon.Shared.Core.Interfaces;
using System.Security.Claims;

namespace Fixeon.WebApi.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITenantContextServices tenantContext)
        {
            var companyClaim = context.User.FindFirst("companyId");

            if (companyClaim != null && Guid.TryParse(companyClaim.Value, out var companyId))
            {
                context.Items["CompanyId"] = companyId;
                tenantContext.TenantId = companyId;
            }

            var organizationIdClaim = context.User.FindFirst("organizationId");

            if(organizationIdClaim != null)
            {
                Guid.TryParse(organizationIdClaim.Value, out var orgId);

                context.Items["OrganizationId"] = orgId;
                tenantContext.OrganizationId = orgId;
            }

            var userIdClaim = context.User.FindFirst("id");
            var userEmailClaim = context.User.FindFirst("email") ?? context.User.FindFirst(ClaimTypes.Email);

            if (userIdClaim != null && userEmailClaim != null)
            {
                Guid.TryParse(userIdClaim.Value, out var userId);
                var userEmail = userEmailClaim.Value;

                context.Items["UserId"] = userId;
                context.Items["UserEmail"] = userEmail;
                tenantContext.UserId = userId;
                tenantContext.UserEmail = userEmail;
            }

            await _next(context);
        }
    }
}
