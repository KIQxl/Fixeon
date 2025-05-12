using Fixeon.Auth.Application.Interfaces;

namespace Fixeon.Auth.Application.Dtos
{
    public record LoginResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
