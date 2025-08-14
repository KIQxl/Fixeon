using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeTenantContext : ITenantContext
    {
        public Guid TenantId { get; set; } = Guid.NewGuid();
        public string? OrganizationName { get; set; } = null;
        public Guid? OrganizationId { get; set; } = null;
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserEmail { get; set; } = Guid.NewGuid().ToString();
    }
}
