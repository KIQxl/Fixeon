namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateOrganizationRequest
    {
        public string Name { get; set; }
        public List<CreateSLAInOrganizationRequest>? Slas { get; set; } 

        public CreateOrganizationRequest(string name, List<CreateSLAInOrganizationRequest>? slas)
        {
            Name = name;
            Slas = slas;
        }
    }
}
