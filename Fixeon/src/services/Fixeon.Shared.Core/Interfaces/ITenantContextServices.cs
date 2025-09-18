using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface ITenantContextServices
    {
        public Guid TenantId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        public Task<CurrentUser> GetCurrentUser();
    }
}