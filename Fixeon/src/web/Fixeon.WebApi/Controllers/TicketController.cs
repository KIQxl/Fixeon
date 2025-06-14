using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.WebApi.Configuration;
using Fixeon.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.WebApi.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketServices _ticketServices;

        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTicketsAsync()
        {
            var response = await _ticketServices.GetAllTicketsAsync();

            if(response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] Guid id)
        {
            var response = await _ticketServices.GetTicketByIdAsync(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("get-by-category/{category}")]
        public async Task<IActionResult> GetTicketsByCategory([FromRoute] string category)
        {
            var response = await _ticketServices.GetTicketsByCategoryAsync(category);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("gey-by-analist-id/{analist}")]
        public async Task<IActionResult> GetTicketsByAnalist([FromRoute] string analist)
        {
            var response = await _ticketServices.GetTicketsByAnalistIdAsync(analist);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("gey-by-user-id/{user}")]
        public async Task<IActionResult> GetTicketsByUser([FromRoute] string user)
        {
            var response = await _ticketServices.GetTicketsByUserIdAsync(user);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpGet]
        [Route("get-interactions-by-ticket-id/{id}")]
        public async Task<IActionResult> GetInteractionsByTicketId([FromRoute] Guid id)
        {
            var response = await _ticketServices.GetInteractionsByTicketIdAsync(id);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPost]
        [Route("create-ticket")]
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
        public async Task<IActionResult> CreateInteraction([FromBody] CreateInteractionRequest request)
        {
            var response = await _ticketServices.CreateTicketInteraction(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPut]
        [Route("assign-ticket")]
        public async Task<IActionResult> AssignTicketToAnalist([FromBody] CreateAssignTicketRequest request)
        {
            var response = await _ticketServices.AssignTicketTo(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }

        [HttpPut]
        [Route("change-ticket-status")]
        public async Task<IActionResult> ChangeTicketStatus([FromBody] ChangeTicketStatusRequest request)
        {
            var response = await _ticketServices.ChangeTicketStatus(request);

            if (response.Success)
                return Ok(response);

            return this.ReturnResponseWithStatusCode(response);
        }
    }
}
