using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixon.Tests.MockRepository
{
    public class FakeTenantContext : ITenantContextServices
    {
        public Guid TenantId { get; set; } = Guid.NewGuid();
        public string? OrganizationName { get; set; } = null;
        public Guid? OrganizationId { get; set; } = null;
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserEmail { get; set; } = Guid.NewGuid().ToString();

        public Task<CurrentUser> GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
