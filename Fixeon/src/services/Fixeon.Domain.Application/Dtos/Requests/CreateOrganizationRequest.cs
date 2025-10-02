using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateOrganizationRequest : IRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public List<CreateSLAInOrganizationRequest>? Slas { get; set; }

        public CreateOrganizationRequest(string name, string cNPJ, string email, List<CreateSLAInOrganizationRequest>? slas)
        {
            Name = name;
            Slas = slas;
            CNPJ = cNPJ;
            Email = email;
        }
    }
}
