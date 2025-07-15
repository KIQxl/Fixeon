using Fixeon.Shared.Interfaces;

namespace Fixeon.Shared.Models
{
    public class TenantContext : ITenantContext
    {
        public Guid TenantId { get; set; }
    }
}
