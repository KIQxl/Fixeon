using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.Interfaces;

namespace Fixeon.Domain.Core.Entities
{
    public class OrganizationsSLA : Entity, ITenantEntity
    {
        public Guid CompanyId { get; private set; }
        public Guid OrganizationId { get; private set; }
        public int SLAInMinutes { get; private set; }
        public string SLAPriority { get; private set; }
        public ESLAType Type { get; private set; }
    }
}