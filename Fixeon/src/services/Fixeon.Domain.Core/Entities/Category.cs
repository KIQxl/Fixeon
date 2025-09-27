using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Core.Entities
{
    public class Category : Entity, ITenantEntity
    {
        public string Name { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid OrganizationId { get; private set; }
        public virtual Organization Organization { get; private set; }

        public Category(string name, Guid organizationId)
        {
            Name = name;
            OrganizationId = organizationId;
        }
    }
}
