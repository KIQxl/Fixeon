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
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageServices _storageServices;

        public OrganizationServices(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IStorageServices storageServices)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
            _storageServices = storageServices;
        }

        // ORGANIZATIONS
        public async Task<Response<List<OrganizationResponse>>> GetAllOrganizations()
        {
            try
            {
                var organizations = await _organizationRepository.GetAllOrganizations();

                var tasks = organizations.Select(async organization =>
                {
                    var presignedUrl = await GetPresignedUrl("organizations/profile_pictures", organization.ProfilePictureUrl);

                    return new OrganizationResponse(
                        organization.Id,
                        organization.Name,
                        organization.CompanyId,
                        organization.CNPJ,
                        organization.Email,
                        presignedUrl,
                        organization.PhoneNumber,
                        organization.Notes,
                        organization.Address,
                        organization.Status,
                        organization.CreatedAt,
                        organization.SLAs,
                        organization.Categories?.ToList(),
                        organization.Departaments?.ToList()
                    );
                });

                var results = await Task.WhenAll(tasks);

                return new Response<List<OrganizationResponse>>(results.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<OrganizationResponse>>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<OrganizationResponse>> GetOrganizationById(Guid organizationId)
        {
            try
            {
                var organization = await _organizationRepository.GetOrganizationById(organizationId);

                if(organization is null)
                    return new Response<OrganizationResponse>("Não encontramos sua organização, verifique com seu suporte se ela não foi desativada ou excluída.", EErrorType.NotFound);

                var profileImageUrl = await GetPresignedUrl("organizations/profile_pictures", organization.ProfilePictureUrl);

                return new Response<OrganizationResponse>(new OrganizationResponse(
                            organization.Id,
                            organization.Name,
                            organization.CompanyId,
                            organization.CNPJ,
                            organization.Email,
                            profileImageUrl,
                            organization.PhoneNumber,
                            organization.Notes,
                            organization.Address,
                            organization.Status,
                            organization.CreatedAt,
                            organization.SLAs,
                            organization.Categories?.ToList(),
                            organization.Departaments?.ToList()));
            }
            catch (Exception ex)
            {
                return new Response<OrganizationResponse>(ex.Message, EErrorType.NotFound);
            }
        }

        public async Task<Response<bool>> CreateOrganization(CreateOrganizationRequest request)
        {
            try
            {
                var validationResult = request.Validate();
                if(!validationResult.IsValid)
                    return new Response<bool>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.ServerError);

                if (request.ProfilePictureUrl != null)
                    await SaveFile(request.ProfilePictureUrl);

                var address = new Address { Street = request.Street, Number = request.Number, Neighborhood = request.Neighborhood, City = request.City, State = request.State, PostalCode = request.PostalCode, Country = request.Country };
                var organization = new Organization(request.Name, request.CNPJ, request.Email, request.PhoneNumber, address, request.Notes, request.ProfilePictureUrl?.FileName);

                if (request.Categories.Any())
                {
                    foreach(var category in request.Categories)
                    {
                        organization.AddCategory(new Category(category, organization.Id));
                    }
                }

                if (request.Departaments.Any())
                {
                    foreach (var departament in request.Departaments)
                    {
                        organization.AddDepartament(new Departament(departament, organization.Id));
                    }
                }

                if (request.Slas != null && request.Slas.Any())
                {
                    foreach (var sla in request.Slas)
                    {
                        var SLA = new OrganizationsSLA(organization.Id, sla.SLAInMinutes, sla.SLAPriority.ToString(), sla.Type);

                        organization.NewSLAConfig(SLA);
                    }
                }

                await _organizationRepository.CreateOrganization(organization);

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> DeleteOrganization(Guid organizationId)
        {
            try
            {
                var organization = await _organizationRepository.GetOrganizationById(organizationId);

                await _organizationRepository.DeleteOrganization(organization);

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message, EErrorType.ServerError);
            }
        }

        // SLA
        public async Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<bool>(validationResult
                    .Errors
                    .Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

            var organization = await _organizationRepository.GetOrganizationById(request.OrganizationId);

            if (organization is null)
                return new Response<bool>("Cliente/Organização não encontrado.", EErrorType.BadRequest);

            var SLA = new OrganizationsSLA(request.OrganizationId, request.SLAInMinutes, request.SLAPriority.ToString(), request.Type);

            var dbSla = organization.SLAs.Where(s => s.SLAPriority == request.SLAPriority.ToString() && s.Type == request.Type).FirstOrDefault();
            if (dbSla != null)
            {
                await _organizationRepository.RemoveSla(dbSla);
            }

            organization.NewSLAConfig(SLA);

            try
            {
                await _organizationRepository.AddOrganizationSLA(SLA);
                await _organizationRepository.UpdateOrganization(organization);
                await _unitOfWork.Commit();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.InnerException.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        //CATEGORY
        public async Task<Response<List<string>>> GetCategories(Guid organizationId)
        {
            try
            {
                var categories = await _organizationRepository.GetCategories(organizationId);

                return new Response<List<string>>(categories.Select(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.InnerException?.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                var validationResult = request.Validate();

                if (!validationResult.IsValid)
                    return new Response<bool>(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

                var organization = await _organizationRepository.GetOrganizationById(request.OrganizationId);

                if (organization is null)
                    return new Response<bool>("Não foi possivel cadastrar a categoria pois a organização não foi encontrada.", EErrorType.NotFound);

                var category = new Category(request.CategoryName, organization.Id);

                await _organizationRepository.CreateCategory(category);
                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<bool>("Não foi possivel cadastrar a categoria.", EErrorType.BadRequest);


                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message ?? ex.InnerException.Message, EErrorType.BadRequest);
            }
        }

        public async Task<Response<bool>> DeleteCategory(DeleteCategoryOrDepartament request)
        {
            try
            {
                var organization = await _organizationRepository.GetOrganizationById(request.OrganizationId);

                if (organization is null)
                    return new Response<bool>("Não foi possivel remover a categoria pois a organização não foi encontrada.", EErrorType.NotFound);

                var category = organization.Categories.FirstOrDefault(x => x.Id == request.CategoryOrDepartamentId);

                if (category is null)
                    return new Response<bool>("Não foi possivel remover a categoria pois a mesma não foi encontrada.", EErrorType.NotFound);

                await _organizationRepository.DeleteCategory(category);

                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<bool>("Não foi possivel remover a categoria.", EErrorType.BadRequest);


                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message ?? ex.InnerException.Message, EErrorType.BadRequest);
            }
        }

        // DEPARTAMENT
        public async Task<Response<bool>> CreateDepartament(CreateDepartamentRequest request)
        {
            try
            {
                var validationResult = request.Validate();

                if (!validationResult.IsValid)
                    return new Response<bool>(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

                var organization = await _organizationRepository.GetOrganizationById(request.OrganizationId);

                if (organization is null)
                    return new Response<bool>("Não foi possivel cadastrar o departamento pois a organização não foi encontrada.", EErrorType.NotFound);

                var departament = new Departament(request.DepartamentName, organization.Id);

                await _organizationRepository.CreateDepartament(departament);
                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<bool>("Não foi possivel cadastrar o departamento.", EErrorType.BadRequest);


                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message ?? ex.InnerException.Message, EErrorType.BadRequest);
            }
        }

        public async Task<Response<List<string>>> GetDepartaments(Guid organizationId)
        {
            try
            {
                var departaments = await _organizationRepository.GetDepartaments(organizationId);

                return new Response<List<string>>(departaments.Select(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.InnerException?.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> DeleteDepartament(DeleteCategoryOrDepartament request)
        {
            try
            {
                var organization = await _organizationRepository.GetOrganizationById(request.OrganizationId);

                if (organization is null)
                    return new Response<bool>("Não foi possivel remover o departamento pois a organização não foi encontrada.", EErrorType.NotFound);

                var departament = organization.Departaments.FirstOrDefault(x => x.Id == request.CategoryOrDepartamentId);

                if (departament is null)
                    return new Response<bool>("Não foi possivel remover o departamento pois o mesmo não foi encontrada.", EErrorType.NotFound);

                await _organizationRepository.DeleteDepartament(departament);

                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<bool>("Não foi possivel remover o departamento.", EErrorType.BadRequest);


                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message ?? ex.InnerException.Message, EErrorType.BadRequest);
            }
        }

        private async Task<string> GetPresignedUrl(string path, string filename)
        {
            var presignedUrl = await _storageServices.GetPresignedUrl(path, filename);

            return presignedUrl;
        }

        private async Task SaveFile(FormFileAdapterDto file)
        {
            try
            {
                await _storageServices.UploadFile("organizations/profile_pictures", file.FileName, file.ContentType, file.Content);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar anexos." + ex.Message);
            }
        }
    }
}
