using Fixeon.Shared.Core.Interfaces;

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
            var companyClaim = context.User.FindFirst("CompanyId");

            if (companyClaim != null && Guid.TryParse(companyClaim.Value, out var companyId))
            {
                context.Items["CompanyId"] = companyId;
                tenantContext.TenantId = companyId;
            }

            await _next(context);
        }
    }
}
