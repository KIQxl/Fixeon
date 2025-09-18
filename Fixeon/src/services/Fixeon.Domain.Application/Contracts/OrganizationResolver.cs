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

        public async Task<CurrentOrganization> GetOrganization(Guid organizationId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);

            if (organization == null)
                return new CurrentOrganization();

            return new CurrentOrganization
            {
                OrganizationId = organizationId,
                OrganizationName = organization.Name,
            };
        }
    }
}
