using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("companies")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyApplicationServices _companyApplicationServices;

        public CompanyController(ICompanyApplicationServices companyApplicationServices)
        {
            _companyApplicationServices = companyApplicationServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyApplicationServices.GetAllCompanies();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var result = await _companyApplicationServices.CreateCompany(request);

            if(result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
