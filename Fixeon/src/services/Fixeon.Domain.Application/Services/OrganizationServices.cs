using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Validator;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Services
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationServices(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }

        // ORGANIZATIONS
        public async Task<Response<List<OrganizationResponse>>> GetAllOrganizations()
        {
            try
            {
                var organizations = await _organizationRepository.GetAllOrganizations();

                return new Response<List<OrganizationResponse>>(organizations
                    .Select(o =>
                        new OrganizationResponse(o.Id, o.Name, o.CompanyId, o.SLAs))
                    .ToList());
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
                return new Response<OrganizationResponse>(new OrganizationResponse(organization.Id, organization.Name, organization.CompanyId, organization.SLAs));
            }
            catch (Exception ex)
            {
                return new Response<OrganizationResponse>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<OrganizationResponse>> CreateOrganization(CreateOrganizationRequest request)
        {
            try
            {
                var organization = new Organization(request.Name);

                if (request.Slas != null && request.Slas.Any())
                {
                    foreach (var sla in request.Slas)
                    {
                        var SLA = new OrganizationsSLA(organization.Id, sla.SLAInMinutes, sla.SLAPriority.ToString(), sla.Type);

                        organization.NewSLAConfig(SLA);
                    }
                }

                await _organizationRepository.CreateOrganization(organization);

                return new Response<OrganizationResponse>(new OrganizationResponse(organization.Id, organization.Name, organization.CompanyId, organization.SLAs));
            }
            catch (Exception ex)
            {
                return new Response<OrganizationResponse>(ex.Message, EErrorType.ServerError);
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
    }
}
