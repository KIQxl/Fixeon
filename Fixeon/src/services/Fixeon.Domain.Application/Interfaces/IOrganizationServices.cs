using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface IOrganizationServices
    {
        public Task<Response<List<OrganizationResponse>>> GetAllOrganizations();
        public Task<Response<OrganizationResponse>> GetOrganizationById(Guid organizationId);
        public Task<Response<OrganizationResponse>> CreateOrganization(CreateOrganizationRequest request);
        public Task<Response<bool>> DeleteOrganization(Guid organizationId);

        //SLA
        public Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request);
    }
}
