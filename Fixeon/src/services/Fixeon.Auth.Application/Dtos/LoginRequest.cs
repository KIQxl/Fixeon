namespace Fixeon.Auth.Application.Dtos
{
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
