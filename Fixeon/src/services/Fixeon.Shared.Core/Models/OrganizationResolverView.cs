namespace Fixeon.Shared.Core.Models
{
    public class OrganizationResolverView
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<CategoryOrDepartamentOrganizationResolverView> Categories { get; set; } = new List<CategoryOrDepartamentOrganizationResolverView>();
        public List<CategoryOrDepartamentOrganizationResolverView> Departaments { get; set; } = new List<CategoryOrDepartamentOrganizationResolverView>();
    }
}
