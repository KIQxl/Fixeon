using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Shared.Models
{
    public class TenantContext : ITenantContext
    {
        public Guid TenantId { get; set; }
    }
}
