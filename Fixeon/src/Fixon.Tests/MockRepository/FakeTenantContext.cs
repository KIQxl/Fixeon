using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeTenantContext : ITenantContext
    {
        public Guid TenantId { get; set; }
        public string? OrganizationName { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set ; }
    }
}
