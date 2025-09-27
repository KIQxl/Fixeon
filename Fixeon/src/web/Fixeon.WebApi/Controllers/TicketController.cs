using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.WebApi.Configuration;
using Fixeon.WebApi.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("tickets")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketServices _ticketServices;

        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }

        [HttpGet]
        [Authorize( Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetAllTicketsAsync([FromQuery] string? category, [FromQuery] string? status, [FromQuery] string? priority, [FromQuery] Guid? analyst, [FromQuery] Guid? user, [FromQuery] string? protocol)
        {
            var response = await _ticketServices.GetAllTicketsFilterAsync(category, status, priority, analyst, user, protocol);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid id)
        {
            var response = await _ticketServices.GetTicketByIdAsync(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("get-interactions-by-ticket-id/{id}")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> GetInteractionsByTicketId([FromRoute] Guid id)
        {
            var response = await _ticketServices.GetInteractionsByTicketIdAsync(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-ticket")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketRequestDto request)
        {
            var requestAdapt = request.ToApplicationRequest();

            var response = await _ticketServices.CreateTicket(requestAdapt);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-interaction")]
        [Authorize(Policy = AuthorizationPolicies.CommonUserPolicy)]
        public async Task<IActionResult> CreateInteraction([FromForm] CreateInteractionRequestDto request)
        {
            var requestAdapt = request.ToApplicationRequest();

            var response = await _ticketServices.CreateTicketInteraction(requestAdapt);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPut]
        [Route("assign-ticket")]
        [Authorize(Policy = AuthorizationPolicies.AnalystPolicy)]
        public async Task<IActionResult> AssignTicketToAnalyst([FromBody] CreateAssignTicketRequest request)
        {
            var response = await _ticketServices.AssignTicketTo(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPut]
        [Route("change-ticket-status")]
        [Authorize(Policy = AuthorizationPolicies.AnalystPolicy)]
        public async Task<IActionResult> ChangeTicketStatus([FromBody] ChangeTicketStatusRequest request)
        {
            var response = await _ticketServices.ChangeTicketStatus(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("get-tickets/analysis")]
        [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
        public async Task<IActionResult> GetTicketsAnalysis()
        {
            var response = await _ticketServices.GetDashboardTickets();

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPut]
        [Route("change-ticket-category")]
        [Authorize(Policy = AuthorizationPolicies.AnalystPolicy)]
        public async Task<IActionResult> ChangeTicketCategory([FromBody] ChangeTicketCategory request)
        {
            var response = await _ticketServices.ChangeTicketCategory(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }
    }
}