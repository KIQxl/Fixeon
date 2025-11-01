using Fixeon.Domain.Application.Interfaces;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Contracts
{
    public class OrganizationResolver : IOrganizationResolver
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationResolver(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<OrganizationResolverView> GetOrganization(Guid organizationId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);

            if (organization == null)
                return new OrganizationResolverView();

            return new OrganizationResolverView
            {
                OrganizationId = organizationId,
                OrganizationName = organization.Name,
                Categories = organization.Categories.Select(x => new CategoryOrDepartamentOrganizationResolverView { Id = x.Id, Name = x.Name }).ToList(),
                Departaments = organization.Departaments.Select(x => new CategoryOrDepartamentOrganizationResolverView { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        public async Task<List<OrganizationResolverView>> GetOrganizations(IEnumerable<Guid> organizationIds)
        {
            var orgs = await _organizationRepository.GetOrganizations(organizationIds);

            return orgs.Select(o => new OrganizationResolverView
            {
                OrganizationId = o.Id,
                OrganizationName = o.Name
            }).ToList();
        }

        public async Task<List<SLAResolverView>> GetSLAByOrganization(Guid organizationId)
        {
            var SLAs = await _organizationRepository.GetSLAByOrganization(organizationId);

            var SLAsView = SLAs.Select(s => new SLAResolverView
            {
                OrganizationId = s.OrganizationId,
                SLAInMinutes = s.SLAInMinutes,
                SLAPriority = s.SLAPriority,
                Type = (int)s.Type
            })
                .ToList();

            return SLAsView;
        }
    }
}
