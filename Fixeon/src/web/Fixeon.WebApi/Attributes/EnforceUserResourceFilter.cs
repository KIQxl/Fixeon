using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Fixeon.WebApi.Attributes
{
    public class EnforceUserResourceFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated == true && user.IsInRole("Usuario"))
            {
                var userIdFromToken = user.FindFirstValue("id");

                if (!string.IsNullOrEmpty(userIdFromToken))
                {
                    var query = context.HttpContext.Request.Query;

                    var newQueryItems = query.ToDictionary(q => q.Key, q => q.Value);

                    newQueryItems["user"] = userIdFromToken;

                    context.HttpContext.Request.Query = new QueryCollection(newQueryItems);
                }
            }
        }
    }
}
