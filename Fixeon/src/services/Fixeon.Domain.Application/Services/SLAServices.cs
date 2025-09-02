using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Validator;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Domain.Application.Services
{
    public class SLAServices : ISLAServices
    {
        private readonly ISLARepository _repository;
        private readonly ITenantContext _tenantContext;
        private readonly IUnitOfWork _unitOfWork;

        public SLAServices(ISLARepository repository, ITenantContext tenantContext, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _tenantContext = tenantContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> CreateOrganizationSLA(CreateSLARequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<bool>(validationResult
                    .Errors
                    .Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

            var SLA = new OrganizationsSLA(_tenantContext.TenantId, request.Organization, request.OrganizationId, request.SLAInMinutes, request.SLAPriority.ToString(), request.Type);

            try
            {
                await _repository.AddOrganizationSLA(SLA);
                await _unitOfWork.Commit();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.InnerException.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<List<OrganizationSLAResponse>>> GetOrganizationSLA(Guid organizationId, EPriority priority)
        {
            var SLAs = await _repository.GetSLAByOrganizationAndPriority(organizationId, priority);

            var response = SLAs.Select(x => new OrganizationSLAResponse
            {
                CompanyId = x.CompanyId,
                OrganizationId = organizationId,
                CreateAt = x.CreateAt,
                ModifiedAt = x.ModifiedAt,
                Organization = x.Organization,
                SLAInMinutes = x.SLAInMinutes,
                SLAPriority = x.SLAPriority,
                Type = x.Type
            }).ToList();

            return new Response<List<OrganizationSLAResponse>>(response);
        }
    }
}
