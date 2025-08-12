namespace Fixeon.Auth.Infraestructure.Dtos.Responses
{
    public class OrganizationResponse
    {
        public OrganizationResponse(Guid id, string name, Guid companyId)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
    }
}
