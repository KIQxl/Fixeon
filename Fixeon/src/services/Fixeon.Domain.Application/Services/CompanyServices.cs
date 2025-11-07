using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ICompanyRepository _repository;
        private readonly IUnitOfWork _UoW;

        public CompanyServices(ICompanyRepository repository, IUnitOfWork uoW)
        {
            _repository = repository;
            _UoW = uoW;
        }

        public async Task<Response<CompanyResponse>> CreateCompany(CreateCompanyRequest request)
        {
            try
            {
                var company = new Company(request.Name, request.CNPJ, request.Email, null);

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

        public async Task<Response<List<TicketTag>>> GetAllTags()
        {
            try
            {
                var tags = await _repository.GetAllTagsByCompany();

                return new Response<List<TicketTag>>(tags.Select(t => new TicketTag { Id = t.Id, Name = t.Name}).ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<TicketTag>>(new List<string> { "Não foi possivel encontrar as tags.", ex.Message }, EErrorType.BadRequest);
            }
        }

        public async Task<Response<bool>> CreateTag(CreateTagRequest request)
        {
            var tag = new Tag(request.Name);

            try
            {
                await _repository.CreateTag(tag);
                await _UoW.Commit();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Não foi possivel criar a tag.", ex.Message }, EErrorType.BadRequest);
            }
        }

        public async Task<Response<bool>> RemoveTag(Guid tagId)
        {
            var tag = await _repository.GetTagById(tagId);

            if(tag is null)
                return new Response<bool>("Tag não encontrada.", EErrorType.NotFound);

            try
            {
                await _repository.RemoveTag(tag);
                await _UoW.Commit();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Não foi possivel remover a tag.", ex.Message }, EErrorType.BadRequest);
            }
        }
    }
}
