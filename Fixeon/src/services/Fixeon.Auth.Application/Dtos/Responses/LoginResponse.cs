using Fixeon.Auth.Application.Interfaces;

namespace Fixeon.Auth.Application.Dtos.Responses
{
    public record LoginResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
