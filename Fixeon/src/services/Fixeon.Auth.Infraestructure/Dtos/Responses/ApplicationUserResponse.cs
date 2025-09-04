namespace Fixeon.Auth.Infraestructure.Dtos.Responses
{
    public class ApplicationUserResponse
    {
        public ApplicationUserResponse()
        {
            
        }

        public ApplicationUserResponse(List<string> errors)
        {
            Errors = errors;
        }

        public ApplicationUserResponse(string error)
        {
            this.Errors.Add(error);
        }

        public ApplicationUserResponse(string id, string username, string email, Guid? organizationId, IList<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            OrganizationId = organizationId;
            Roles = roles;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid? OrganizationId { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
