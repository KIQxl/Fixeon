using Fixeon.Auth.Application.Dtos;
using Fixeon.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _services;

        public AuthenticationController(IAuthenticationServices services)
        {
            _services = services;
        }

        [HttpPost]
        [Route("create-account")]

        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.CreateAccount(request);

            if(response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.Login(request);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
