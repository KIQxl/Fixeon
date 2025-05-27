namespace Fixeon.Auth.Application.Dtos
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            
        }

        public ApplicationUser(List<string> errors)
        {
            Errors = errors;
        }

        public ApplicationUser(string id, string username, string email, IList<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            Roles = roles;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
