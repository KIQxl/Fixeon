using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.Interfaces;

namespace Fixeon.Domain.Core.Entities
{
    public class OrganizationsSLA : Entity, ITenantEntity
    {
        private OrganizationsSLA() { }
        public OrganizationsSLA(Guid companyId, string organization, Guid organizationId, int sLAInMinutes, string sLAPriority, ESLAType type)
        {
            CompanyId = companyId;
            Organization = organization;
            OrganizationId = organizationId;
            SLAInMinutes = sLAInMinutes;
            SLAPriority = sLAPriority;
            Type = type;
            CreateAt = DateTime.Now;
        }

        public Guid CompanyId { get; private set; }
        public string Organization { get; private set; }
        public Guid OrganizationId { get; private set; }
        public int SLAInMinutes { get; private set; }
        public string SLAPriority { get; private set; }
        public ESLAType Type { get; private set; }
        public DateTime CreateAt { get; private set; }
        public DateTime? ModifiedAt { get; private set; }
    }
}