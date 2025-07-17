using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface ICompanyService
    {
        public Task<CompanyResponse> CreateCompany(CreateCompanyRequest request);
    }
}
