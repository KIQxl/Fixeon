namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
