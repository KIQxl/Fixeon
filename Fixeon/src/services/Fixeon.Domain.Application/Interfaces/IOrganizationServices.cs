using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface IOrganizationServices
    {
        public Task<Response<List<OrganizationResponse>>> GetAllOrganizations();
        public Task<Response<OrganizationResponse>> GetOrganizationById(Guid organizationId);
        public Task<Response<bool>> CreateOrganization(CreateOrganizationRequest request);
        public Task<Response<bool>> DeleteOrganization(Guid organizationId);

        //SLA
        public Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request);

        // CATEGORY
        public Task<Response<bool>> CreateCategory(CreateCategoryRequest request);
        public Task<Response<List<string>>> GetCategories(Guid organizationId);
        public Task<Response<bool>> DeleteCategory(DeleteCategoryOrDepartament request);

        // DEPARTAMENT
        public Task<Response<bool>> CreateDepartament(CreateDepartamentRequest request);
        public Task<Response<List<string>>> GetDepartaments(Guid organizationId);
        public Task<Response<bool>> DeleteDepartament(DeleteCategoryOrDepartament request);
    }
}
