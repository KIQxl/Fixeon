using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface ICompanyServices
    {
        public Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request);
        public Task<Response<List<CompanyResponse>>> GetAllCompanies();

    }
}
