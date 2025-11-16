using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Validator;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.ValueObjects;
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

        public async Task<Response<CompanyResponse>> GetCompanyById(Guid id)
        {
            try
            {
                var company = await _repository.GetCompanyById(id);

                if (company is null)
                    return new Response<CompanyResponse>("Empresa não encontrada.", EErrorType.BadRequest);

                return new Response<CompanyResponse>(new CompanyResponse(
                    company.Id, 
                    company.Name, 
                    company.CNPJ,
                    company.Email,
                    company.Address, 
                    company.PhoneNumber, 
                    company.ProfilePictureUrl,
                    company.Status,
                    company.CreatedAt,
                    company.Tags, 
                    company.Organizations
                        .Select(x => 
                            new OrganizationResponse
                            (
                                x.Id,
                                x.Name,
                                x.CompanyId,
                                x.CNPJ,
                                x.Email,
                                x.PhoneNumber,
                                x.Notes,
                                x.Address,
                                x.Status,
                                x.CreatedAt,
                                new List<OrganizationsSLA>(),
                                new List<Category>(),
                                new List<Departament>()
                            )).ToList()
                        )
                    );
            }
            catch (Exception ex)
            {
                return new Response<CompanyResponse>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> CreateCompany(CreateCompanyRequest request)
        {
            try
            {
                var validationResult = request.Validate();

                if (!validationResult.IsValid)
                    return new Response<bool>(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

                var address = new Address { Street = request.Street, Number = request.Number, Neighborhood = request.Neighborhood, City = request.City, State = request.State, PostalCode = request.PostalCode, Country = request.Country };
                var company = new Company(request.Name, request.CNPJ, request.Email, request.PhoneNumber, address, null, request.ProfilePicture);

                await _repository.CreateCompany(company);

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<List<CompanyResponse>>> GetAllCompanies()
        {
            try
            {
                var companies = await _repository.GetAllCompanies();

                var result = companies.Select(c => new CompanyResponse(c.Id, c.Name, c.CNPJ, c.Email, c.Address, c.PhoneNumber, c.ProfilePictureUrl, c.Status, c.CreatedAt, null, null)).ToList();

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
