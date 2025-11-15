using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class OrganizationResponse
    {
        public OrganizationResponse(Guid id, string name, Guid companyId, string cNPJ, string email, DateTime createdAt, List<OrganizationsSLA>? SLAs, List<Category>? categories, List<Departament>? departaments)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
            CNPJ = cNPJ;
            Email = email;
            CreatedAt = createdAt;
            OrganizationSLAs = SLAs ?? new List<OrganizationsSLA>();
            Categories = categories ?? new List<Category>();
            Departaments = departaments ?? new List<Departament>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CompanyId { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Departament> Departaments { get; set; } = new List<Departament>();
        public List<OrganizationsSLA> OrganizationSLAs { get; set; } = new List<OrganizationsSLA>();
    }
}
