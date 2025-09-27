using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Core.Entities
{
    public class Departament : Entity, ITenantEntity
    {
        public Departament(string name, Guid organizationId)
        {
            Name = name;
            OrganizationId = organizationId;
        }

        public Guid CompanyId { get; private set; }
        public string Name { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; }
    }
}
