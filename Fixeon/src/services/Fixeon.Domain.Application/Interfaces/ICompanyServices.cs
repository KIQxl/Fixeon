using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ICompanyServices
    {
        public Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request);
        public Task<Response<List<CompanyResponse>>> GetAllCompanies();

        // TAGS
        public Task<Response<bool>> CreateTag(CreateTagRequest request);
        public Task<Response<bool>> RemoveTag(Guid tagId);
    }
}
