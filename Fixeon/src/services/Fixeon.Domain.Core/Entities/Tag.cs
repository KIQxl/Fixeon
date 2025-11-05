using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Core.Entities
{
    public class Tag : Entity, ITenantEntity
    {
        public Tag(string name)
        {
            Name = name;
        }

        public Guid CompanyId { get; private set; }
        public virtual Company Company { get; private set; }
        public string Name { get; private set; }
        public virtual List<Ticket> Tickets { get; private set; }
    }
}
