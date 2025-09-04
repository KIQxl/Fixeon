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

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var result = await _companyApplicationServices.CreateCompany(request);

            if(result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
