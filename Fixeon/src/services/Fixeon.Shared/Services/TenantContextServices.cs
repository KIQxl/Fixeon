using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Models
{
    public class TenantContextServices : ITenantContextServices
    {
        public TenantContextServices()
        {
        }

        public Guid TenantId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        public async Task<CurrentUser> GetCurrentUser()
        {
            return new CurrentUser
            {
                TenantId = TenantId,
                OrganizationId = OrganizationId.HasValue ? OrganizationId.Value : Guid.Empty,
                UserId = UserId,
                UserEmail = UserEmail
            };
        }
    }
}
