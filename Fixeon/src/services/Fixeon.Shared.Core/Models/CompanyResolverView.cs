namespace Fixeon.Shared.Core.Models
{
    public class CompanyResolverView
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string CompanyEmail { get; set;}
        public List<CompanyTagResolverView> Tags { get; set; }
    }
}
