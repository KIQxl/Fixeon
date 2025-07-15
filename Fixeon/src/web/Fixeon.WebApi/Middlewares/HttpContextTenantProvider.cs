using Fixeon.Shared.Interfaces;

namespace Fixeon.WebApi.Middlewares
{
    public class HttpContextTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextTenantProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetTenantId()
        {
            var context = _contextAccessor.HttpContext;

            if (context?.Items["companyId"] is Guid companyId)
                return companyId;

            throw new Exception("Tenant Id não foi definido no contexto.");
        }
    }
}
