using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Entities
{
    public class Organization : Entity, ITenantEntity
    {
        public Organization() { }
        public Organization(string name, string cnpj, string email, string phoneNumber, Address address, string? notes, string? profilePictureUrl)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cnpj;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Notes = notes ?? string.Empty;
            CreatedAt = DateTime.Now;
            Status = EActiveStatus.Onboarding;
            SLAs = new List<OrganizationsSLA>();
            Categories = new List<Category>();
            Departaments = new List<Departament>();
            ProfilePictureUrl = profilePictureUrl;
        }

        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }
        public Address Address { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? Notes { get; private set; }
        public string? ProfilePictureUrl { get; set; }
        public EActiveStatus Status { get; private set; }
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
