using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Models
{
    public class TenantContextServices : ITenantContextServices
    {
        private readonly IOrganizationResolver _orgResolver;

        public TenantContextServices(IOrganizationResolver orgResolver)
        {
            _orgResolver = orgResolver;
        }

        public Guid TenantId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        public async Task<CurrentUser> GetCurrentUser()
        {
            var org = await _orgResolver.GetOrganization(this.OrganizationId.Value);

            return new CurrentUser
            {
                UserId = UserId,
                UserEmail = UserEmail,
                Organization = org
            };
        }
    }
}
