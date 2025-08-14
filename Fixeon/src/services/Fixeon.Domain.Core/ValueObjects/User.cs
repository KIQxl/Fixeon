namespace Fixeon.Domain.Core.ValueObjects
{
    public record User
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string? OrganizationName { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
