using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateOrganizationRequest : IRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Departaments { get; set; } = new List<string>();
        public List<CreateSLAInOrganizationRequest> Slas { get; set; } = new List<CreateSLAInOrganizationRequest>();
    }
}
