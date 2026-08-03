namespace Fixeon.Shared.Core.Models
{
    public class CurrentUser
    {
        public Guid TenantId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
