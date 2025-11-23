using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.WebApi.Dtos.Requests;
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
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> CreateAccount([FromForm] CreateApplicationUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var requestDto = request.ToApplicationRequest();

            var response = await _services.CreateIdentityUser(requestDto);

            if(response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut]
        [Route("update-account")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateApplicationUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<ApplicationUserResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.UpdateApplicationUser(request);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost]
        [Route("associate-role")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> AssociateRole([FromBody] AssociateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<LoginResponse>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()));

            var response = await _services.AssociateRole(request.UserId, request.Roles);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-user-by-id/{id}")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await _services.GetuserById(id);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-users")]
        [Authorize(Policy = AuthorizationPolicies.AnalystPolicy)]
        public async Task<IActionResult> GetAllUsers([FromQuery] Guid? id, [FromQuery] string? email, [FromQuery] Guid? organization, [FromQuery] string? username)
        {
            var response = await _services.GetAllUsers(id, email, organization, username);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-roles")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _services.GetAllRoles();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("analysts")]
        [Authorize(Policy = AuthorizationPolicies.AnalystPolicy)]
        public async Task<IActionResult> GetAllAnalysts()
        {
            var response = await _services.GetUserByRoleName("Analista");

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
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]
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
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]
        public async Task<IActionResult> CreateFirstUserForCompany([FromForm] CreateApplicationUserDto request)
        {
            var applicationRequest = request.ToApplicationRequest();

            var response = await _services.MasterAdminCreateFirstForCompany(applicationRequest);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet]
        [Route("get-all-master")]
        [Authorize(Policy = AuthorizationPolicies.MasterAdminPolicy)]
        public async Task<IActionResult> MasterAdminGetAllUsers()
        {
            var response = await _services.MasterAdminGetAllUsers();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        //[HttpGet]
        //[Route("get-organizations")]
        //[Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        //public async Task<IActionResult> GetAllOrganizations()
        //{
        //    var response = await _services.GetAllOrganizations();

        //    if (response.Success)
        //        return Ok(response);

        //    return BadRequest(response);
        //}

        //[HttpPost]
        //[Route("create-organization")]
        //[Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        //public async Task<IActionResult> CreateOrganization(CreateOrganizationRequest request)
        //{
        //    var response = await _services.CreateOrganization(request);

        //    if (response.Success)
        //        return Ok(response);

        //    return BadRequest(response);
        //}

        //[HttpPost]
        //[Route("delete-organization/{id}")]
        //[Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        //public async Task<IActionResult> DeleteOrganization([FromRoute] Guid organizationId)
        //{
        //    var response = await _services.DeleteOrganization(organizationId);

        //    if (response.Success)
        //        return Ok(response);

        //    return BadRequest(response);
        //}
    }
}
