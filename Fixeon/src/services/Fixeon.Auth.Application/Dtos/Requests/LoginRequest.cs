namespace Fixeon.Auth.Application.Dtos.Requests
{
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
