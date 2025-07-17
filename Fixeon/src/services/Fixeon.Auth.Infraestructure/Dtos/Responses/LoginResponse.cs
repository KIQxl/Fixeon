namespace Fixeon.Auth.Infraestructure.Dtos.Responses
{
    public record LoginResponse
    {
        public LoginResponse(string id, string username, string email, string token, IList<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            Token = token;
            Roles = roles;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
