using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;

namespace Fixon.Tests.MockRepository
{
    public class FakeOrganizationServices : IOrganizationServices
    {
        public Task<Response<bool>> CreateCategory(CreateCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> CreateDepartament(CreateDepartamentRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<OrganizationResponse>> CreateOrganization(CreateOrganizationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> DeleteCategory(DeleteCategoryOrDepartament request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> DeleteDepartament(DeleteCategoryOrDepartament request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> DeleteOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<OrganizationResponse>>> GetAllOrganizations()
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<string>>> GetCategories(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<string>>> GetDepartaments(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<OrganizationResponse>> GetOrganizationById(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrganizationsSLA>> GetSLAByOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        Task<Response<bool>> IOrganizationServices.CreateOrganization(CreateOrganizationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
