using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ISLAServices
    {
        public Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request);
        public Task<Response<List<OrganizationSLAResponse>>> GetOrganizationSLA(Guid organizationId, EPriority priority);
    }
}
