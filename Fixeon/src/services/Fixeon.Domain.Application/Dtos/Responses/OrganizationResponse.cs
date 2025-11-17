using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class OrganizationResponse
    {
        public OrganizationResponse(Guid id, string name, Guid companyId, string cnpj, string email, string profileUrl, string phoneNumber, string notes, Address address, EActiveStatus status, DateTime createdAt, List<OrganizationsSLA>? SLAs, List<Category>? categories, List<Departament>? departaments)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
            CNPJ = cnpj;
            Email = email;
            PhoneNumber = phoneNumber;
            Notes = notes;
            Address = address;
            Status = status;
            CreatedAt = createdAt;
            OrganizationSLAs = SLAs ?? new List<OrganizationsSLA>();
            Categories = categories ?? new List<Category>();
            Departaments = departaments ?? new List<Departament>();
            ProfileUrl = profileUrl;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public Address Address { get; set; }
        public EActiveStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CompanyId { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Departament> Departaments { get; set; } = new List<Departament>();
        public List<OrganizationsSLA> OrganizationSLAs { get; set; } = new List<OrganizationsSLA>();
    }
}
