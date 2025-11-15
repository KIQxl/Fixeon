using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("companies")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServices _companyApplicationServices;

        public CompanyController(ICompanyServices companyApplicationServices)
        {
            _companyApplicationServices = companyApplicationServices;
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]

        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyApplicationServices.GetAllCompanies();

            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]

        public async Task<IActionResult> GetCompanyById([FromRoute] Guid id)
        {
            var result = await _companyApplicationServices.GetCompanyById(id);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var result = await _companyApplicationServices.CreateCompany(request);

            return Ok(result);
        }

        [HttpGet("get-tags")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]

        public async Task<IActionResult> GetAllTags()
        {
            var result = await _companyApplicationServices.GetAllTags();

            return Ok(result);
        }

        [HttpPost("create-tag")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
        {
            var result = await _companyApplicationServices.CreateTag(request);

            return Ok(result);
        }

        [HttpDelete("remove-tag/{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> RemoveTag([FromRoute] Guid id)
        {
            var result = await _companyApplicationServices.RemoveTag(id);

            return Ok(result);
        }
    }
}
