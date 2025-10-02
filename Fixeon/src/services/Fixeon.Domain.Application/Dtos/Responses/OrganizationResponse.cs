using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class OrganizationResponse
    {
        public OrganizationResponse(Guid id, string name, Guid companyId, string cNPJ, string email, DateTime createdAt, List<OrganizationsSLA> SLAs, List<string> categories, List<string> departaments)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
            CNPJ = cNPJ;
            Email = email;
            CreatedAt = createdAt;
            OrganizationSLAs = SLAs;
            Categories = categories;
            Departaments = departaments;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CompanyId { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Departaments { get; set; } = new List<string>();
        public List<OrganizationsSLA> OrganizationSLAs { get; set; } = new List<OrganizationsSLA>();
    }
}
