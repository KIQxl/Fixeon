using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Interfaces;
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
    }
}
