namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateOrganizationRequest
    {
        public string Name { get; set; }

        public CreateOrganizationRequest(string name)
        {
            Name = name;
        }
    }
}
