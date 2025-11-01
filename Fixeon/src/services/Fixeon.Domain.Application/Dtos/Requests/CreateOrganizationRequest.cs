using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateOrganizationRequest : IRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Departaments { get; set; } = new List<string>();
        public List<CreateSLAInOrganizationRequest> Slas { get; set; } = new List<CreateSLAInOrganizationRequest>();

        public CreateOrganizationRequest(string name, string cNPJ, string email, List<string> categories, List<string> departaments, List<CreateSLAInOrganizationRequest>? slas)
        {
            Name = name;
            Slas = slas;
            CNPJ = cNPJ;
            Email = email;
            Categories = categories;
            Departaments = departaments;
        }
    }
}
