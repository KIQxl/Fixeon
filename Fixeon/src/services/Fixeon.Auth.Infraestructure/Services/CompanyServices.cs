using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class CompanyServices : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyServices(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<CompanyResponse> CreateCompany(CreateCompanyRequest request)
        {
            try
            {
                var company = new Company(request.Name, request.CNPJ);

                await _repository.CreateCompany(company);

                return new CompanyResponse(company.Id, company.Name, company.CNPJ);
            }
            catch (Exception ex)
            {
                return new CompanyResponse(ex.Message);
            }
        }

        public async Task<List<CompanyResponse>> GetAllCompanies()
        {
            try
            {
                var companies = await _repository.GetAllCompanies();

                return companies.Select(x => new CompanyResponse(x.Id, x.Name, x.CNPJ)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
