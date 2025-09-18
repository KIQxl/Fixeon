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

        public ApplicationUserResponse(string id, string username, string email, UserOrganizationResponse? organization, IList<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            Organization = organization;
            Roles = roles;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserOrganizationResponse? Organization { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
