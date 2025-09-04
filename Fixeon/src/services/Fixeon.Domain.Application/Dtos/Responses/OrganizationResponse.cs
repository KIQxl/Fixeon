using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class OrganizationResponse
    {
        public OrganizationResponse(Guid id, string name, Guid companyId, List<OrganizationsSLA> SLAs)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
            OrganizationSLAs = SLAs;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public List<OrganizationsSLA> OrganizationSLAs { get; set; } = new List<OrganizationsSLA>();
    }
}
