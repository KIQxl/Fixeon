using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Services;
using Fixeon.WebApi.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("organizations")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationServices _organizationServices;

        public OrganizationController(IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }

        [HttpGet]
        //[Route("")]
        //[Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var response = await _organizationServices.GetAllOrganizations();

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        //[Route("")]
        //[Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationRequest request)
        {
            var response = await _organizationServices.CreateOrganization(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetOrganizationById([FromRoute] Guid id)
        {
            var response = await _organizationServices.GetOrganizationById(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> DeleteOganization([FromRoute] Guid id)
        {
            var response = await _organizationServices.DeleteOrganization(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-sla")]
        //[Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> CreateOrganizationSLA([FromBody] CreateSLARequest request)
        {
            var response = await _organizationServices.CreateOrganizationSLA(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-category")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var response = await _organizationServices.CreateCategory(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("categories/{organizationId}")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetAllCategories([FromRoute] Guid organizationId)
        {
            var response = await _organizationServices.GetCategories(organizationId);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-departament")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> CreateDepartament([FromBody] CreateDepartamentRequest request)
        {
            var response = await _organizationServices.CreateDepartament(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("departaments/{organizationId}")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetAllDepartaments([FromRoute] Guid organizationId)
        {
            var response = await _organizationServices.GetDepartaments(organizationId);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }
    }
}
