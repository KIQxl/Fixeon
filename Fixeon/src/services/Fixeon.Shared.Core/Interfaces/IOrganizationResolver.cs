using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface IOrganizationResolver
    {
        public Task<OrganizationResolverView> GetOrganization(Guid organizationId);
        public Task<List<OrganizationResolverView>> GetOrganizations(IEnumerable<Guid> organizationIds);
        public Task<List<SLAResolverView>> GetSLAByOrganization(Guid organizationId);
    }
}
