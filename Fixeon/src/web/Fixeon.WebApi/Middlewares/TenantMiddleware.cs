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

        public async Task Invoke(HttpContext context, ITenantContext tenantContext)
        {
            var companyClaim = context.User.FindFirst("companyId");

            if (companyClaim != null && Guid.TryParse(companyClaim.Value, out var companyId))
            {
                context.Items["CompanyId"] = companyId;
                tenantContext.TenantId = companyId;
            }

            var organizationIdClaim = context.User.FindFirst("organizationId");
            var organizationNameClaim = context.User.FindFirst("organizationName");

            if(organizationIdClaim != null && organizationNameClaim != null)
            {
                Guid.TryParse(organizationIdClaim.Value, out var orgId);
                var orgName = organizationNameClaim.Value;

                context.Items["OrganizationId"] = orgId;
                context.Items["OrganizationName"] = orgName;
                tenantContext.OrganizationId = orgId;
                tenantContext.OrganizationName = orgName;
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
