namespace Fixeon.Shared.Core.Models
{
    public class CurrentUser
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public CurrentOrganization Organization { get; set; }
    }
}
