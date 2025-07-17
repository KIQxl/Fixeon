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
            var company = new Company(request.Name, request.CNPJ);

            await _repository.CreateCompany(company);

            return new CompanyResponse(company.Id, company.Name, company.CNPJ);
        }
    }
}
