using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface ICompanyApplicationServices
    {
        public Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request);
        public Task<Response<List<CompanyResponse>>> GetAllCompanies();
    }
}
