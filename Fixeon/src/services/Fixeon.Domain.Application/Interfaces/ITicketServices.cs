using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ITicketServices
    {
        public Task<Response<TicketResponse>> CreateTicket(CreateTicketRequest request);
        public Task<Response<TicketResponse>> CreateTicketInteraction(CreateInteractionRequest request);
        public Task<Response<TicketResponse>> GetTicketByIdAsync(Guid id);
        public Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsAsync();
        public Task<Response<IEnumerable<InteractionResponse>>> GetInteractionsByTicketIdAsync(Guid ticketId);
        public Task<Response<TicketResponse>> AssignTicketTo(CreateAssignTicketRequest request);
        public Task<Response<TicketResponse>> ChangeTicketStatus(ChangeTicketStatusRequest request);
        public Task<Response<TicketResponse>> ChangeTicketCategory(ChangeTicketCategory request);
        public Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user, string? protocol);
        public Task<Response<TicketDashboardResponse>> GetDashboardTickets();
    }
}
