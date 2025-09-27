using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Interfaces;

namespace Fixeon.Domain.Entities
{
    public class Organization : Entity, ITenantEntity
    {
        public Organization(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            SLAs = new List<OrganizationsSLA>();
            Categories = new List<Category>();
            Departaments = new List<Departament>();
        }

        public string Name { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }
        public virtual List<OrganizationsSLA> SLAs { get; private set; }
        public virtual List<Category> Categories { get; private set; }
        public virtual List<Departament> Departaments { get; private set; }

        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }

        public void AddDepartament(Departament departament)
        {
            Departaments.Add(departament);
        }

        public void NewSLAConfig(OrganizationsSLA SLA)
        {
            SLAs.Add(SLA);
        }
    }
}
