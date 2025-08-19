using Fixeon.Domain.Core.Interfaces;

namespace Fixeon.Domain.Core.Entities
{
    public class Category : Entity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
