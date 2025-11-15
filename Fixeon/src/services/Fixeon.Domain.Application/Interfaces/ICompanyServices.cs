using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ICompanyServices
    {
        public Task<Response<bool>> CreateCompany(CreateCompanyRequest request);
        public Task<Response<List<CompanyResponse>>> GetAllCompanies();
        public Task<Response<CompanyResponse>> GetCompanyById(Guid id);

        // TAGS
        public Task<Response<List<TicketTag>>> GetAllTags();
        public Task<Response<bool>> CreateTag(CreateTagRequest request);
        public Task<Response<bool>> RemoveTag(Guid tagId);
    }
}
