namespace Fixeon.Shared.Core.Models
{
    public class SLAResolverView
    {
        public Guid OrganizationId { get; set; }
        public int SLAInMinutes { get; set; }
        public string SLAPriority { get; set; }
        public int Type { get; set; }
    }
}
