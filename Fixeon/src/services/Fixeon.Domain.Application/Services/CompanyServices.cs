using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Validator;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.ValueObjects;
using Fixeon.Domain.Entities;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ICompanyRepository _repository;
        private readonly IUnitOfWork _UoW;
        private readonly IStorageServices _storageServices;

        public CompanyServices(ICompanyRepository repository, IUnitOfWork uoW, IStorageServices storageServices)
        {
            _repository = repository;
            _UoW = uoW;
            _storageServices = storageServices;
        }

        public async Task<Response<CompanyResponse>> GetCompanyById(Guid id)
        {
            try
            {
                var company = await _repository.GetCompanyById(id);

                if (company is null)
                    return new Response<CompanyResponse>("Empresa não encontrada.", EErrorType.BadRequest);

                var profileImageUrl = await GetPresignedUrl(company.ProfilePictureUrl);

                var orgTasks = (company.Organizations ?? Enumerable.Empty<Organization>())
                    .Select(async x =>
                    {
                        var orgImageUrl = await GetPresignedUrl(x.ProfilePictureUrl);

                        return new OrganizationResponse(x.Id,
                            x.Name,
                            x.CompanyId,
                            x.CNPJ,
                            x.Email,
                            orgImageUrl,
                            x.PhoneNumber,
                            x.Notes,
                            x.Address,
                            x.Status,
                            x.CreatedAt,
                            new List<OrganizationsSLA>(),
                            new List<Category>(),
                            new List<Departament>()
                        );
                    });

                var orgResponses = (await Task.WhenAll(orgTasks)).ToList();

                return new Response<CompanyResponse>
                    (new CompanyResponse(
                    company.Id,
                    company.Name,
                    company.CNPJ,
                    company.Email,
                    company.Address,
                    company.PhoneNumber,
                    profileImageUrl,
                    company.Status,
                    company.CreatedAt,
                    company.Tags,
                    orgResponses)
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

                if (request.ProfilePicture != null)
                    await SaveFile(request.ProfilePicture);

                var address = new Address { Street = request.Street, Number = request.Number, Neighborhood = request.Neighborhood, City = request.City, State = request.State, PostalCode = request.PostalCode, Country = request.Country };
                var company = new Company(request.Name, request.CNPJ, request.Email, request.PhoneNumber, address, null, request.ProfilePicture.FileName);

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

                var tasks = companies.Select(async c =>
                {
                    var url = await GetPresignedUrl(c.ProfilePictureUrl);

                    return new CompanyResponse(
                        c.Id,
                        c.Name,
                        c.CNPJ,
                        c.Email,
                        c.Address,
                        c.PhoneNumber,
                        url,
                        c.Status,
                        c.CreatedAt,
                        null,
                        null
                    );
                });

                var results = await Task.WhenAll(tasks);

                return new Response<List<CompanyResponse>>(results.ToList());
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

                return new Response<List<TicketTag>>(tags.Select(t => new TicketTag { Id = t.Id, Name = t.Name }).ToList());
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

            if (tag is null)
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

        private async Task<string> GetPresignedUrl(string filename)
        {
            var presignedUrl = await _storageServices.GetPresignedUrl(filename);

            return presignedUrl;
        }

        private async Task SaveFile(FormFileAdapterDto file)
        {
            try
            {
                await _storageServices.UploadFile("profile_pictures", file.FileName, file.ContentType, file.Content);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar anexos." + ex.Message);
            }
        }
    }
}
