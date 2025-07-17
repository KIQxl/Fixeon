using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;

namespace Fixeon.Auth.Application.Services
{
    public class CompanyApplicationServices : ICompanyApplicationServices
    {
        private readonly ICompanyService _services;

        public CompanyApplicationServices(ICompanyService services)
        {
            _services = services;
        }

        public async Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request)
        {
            var response = await _services.CreateCompany(request);

            if (response.Errors.Any())
                return new Response<CompanyResponse>(response.Errors);

            return new Response<CompanyResponse>(response);
        }

        public async Task<Response<List<CompanyResponse>>> GetAllCompanies()
        {
            try
            {
                var result = await _services.GetAllCompanies();

                return new Response<List<CompanyResponse>>(result);
            }
            catch (Exception ex)
            {
                return new Response<List<CompanyResponse>>(ex.Message);
            }
        }
    }
}
