using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Core.Entities
{
    public class OrganizationsSLA : Entity
    {
        private OrganizationsSLA() { }
        public OrganizationsSLA(Guid organizationId, int sLAInMinutes, string sLAPriority, ESLAType type)
        {
            OrganizationId = organizationId;
            SLAInMinutes = sLAInMinutes;
            SLAPriority = sLAPriority;
            Type = type;
            CreateAt = DateTime.Now;
        }

        public Organization Organization { get; private set; }
        public Guid OrganizationId { get; private set; }
        public int SLAInMinutes { get; private set; }
        public string SLAPriority { get; private set; }
        public ESLAType Type { get; private set; }
        public DateTime CreateAt { get; private set; }
        public DateTime? ModifiedAt { get; private set; }
    }
}