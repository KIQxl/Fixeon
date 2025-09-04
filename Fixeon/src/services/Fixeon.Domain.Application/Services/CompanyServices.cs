using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ICompanyRepository _repository;

        public CompanyServices(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request)
        {
            try
            {
                var company = new Company(request.Name, request.CNPJ, request.Email);

                await _repository.CreateCompany(company);

                return new Response<CompanyResponse>(new CompanyResponse(company.Id, company.Name, company.CNPJ));
            }
            catch (Exception ex)
            {
                return new Response<CompanyResponse>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<List<CompanyResponse>>> GetAllCompanies()
        {
            try
            {
                var companies = await _repository.GetAllCompanies();

                var result = companies.Select(c => new CompanyResponse(c.Id, c.Name, c.CNPJ)).ToList();

                return new Response<List<CompanyResponse>>(result);
            }
            catch (Exception ex)
            {
                return new Response<List<CompanyResponse>>(ex.Message, EErrorType.ServerError);

            }
        }
    }
}
