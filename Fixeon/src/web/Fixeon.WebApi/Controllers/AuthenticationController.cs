using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.WebApi.Dtos.Responses;
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

        [HttpPost]
        [Route("associate-role")]
        public async Task<IActionResult> AssociateRole([FromBody] AssociateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.AssociateRole(request.UserId, request.RoleName);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<bool>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.CreateRole(request.RoleName);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserEmailId([FromRoute] string id)
        {
            var response = await _services.GetUserByIdAsync(id);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _services.GetAllUsersAsync();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("recovery-password")]
        public async Task<IActionResult> SendRecoveryPasswordLink([FromBody] RecoveryEmailDto request)
        {
            var response = await _services.SendRecoveryPasswordLink(request.Email);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _services.ResetPassword(request);

            if(response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
