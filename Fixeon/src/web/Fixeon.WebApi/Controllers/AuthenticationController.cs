using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.WebApi.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityServices _services;

        public AuthenticationController(IIdentityServices services)
        {
            _services = services;
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

            var response = await _services.Login(request.Email, request.Password);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("create-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.CreateIdentityUser(request);

            if(response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("associate-role")]
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Route("get-user-by-id/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await _services.GetuserById(id);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _services.GetAllUsers();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("recovery-password")]
        public async Task<IActionResult> SendRecoveryPasswordLink([FromBody] RecoveryEmailDto request)
        {
            var response = await _services.GenerateResetPasswordToken(request.Email);

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

        [HttpPost]
        [Route("create-role")]
        [Authorize(Roles = "MasterAdmin")]
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

        [HttpPost]
        [Route("create-account-master")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<IActionResult> CreateFirstUserForCompany([FromBody] CreateAccountRequest request)
        {
            var response = await _services.MasterAdminCreateFirstForCompany(request);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-master")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<IActionResult> MasterAdminGetAllUsers()
        {
            var response = await _services.MasterAdminGetAllUsers();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
