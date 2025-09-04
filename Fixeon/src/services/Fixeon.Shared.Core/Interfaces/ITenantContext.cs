namespace Fixeon.Shared.Core.Interfaces
{
    public interface ITenantContext
    {
        public Guid TenantId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
