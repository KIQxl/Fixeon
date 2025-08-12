namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public record UpdateApplicationUserRequest
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
