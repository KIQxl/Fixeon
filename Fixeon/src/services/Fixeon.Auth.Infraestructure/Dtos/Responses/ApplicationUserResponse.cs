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

        public ApplicationUserResponse(string id, string username, string email, string phoneNumber, string? jobTitle, string? profilePictureUrl, UserOrganizationResponse? organization, IList<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            Organization = organization;
            Roles = roles;
            PhoneNumber = phoneNumber;
            JobTitle = jobTitle;
            ProfilePictureUrl = profilePictureUrl;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public UserOrganizationResponse? Organization { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
