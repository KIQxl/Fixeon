using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ITicketServices
    {
        public Task<Response<TicketResponse>> CreateTicket(CreateTicketRequest request);
        public Task<Response<TicketResponse>> CreateTicketInteraction(CreateInteractionRequest request);
        public Task<Response<TicketResponse>> GetTicketByIdAsync(Guid id);
        public Task<Response<IEnumerable<TicketResponse>>> GetTicketsByPriorityAsync(EPriority priority);
        public Task<Response<IEnumerable<TicketResponse>>> GetTicketsByCategoryAsync(string category);
        public Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsAsync();
        public Task<Response<IEnumerable<TicketResponse>>> GetTicketsByUserIdAsync(string userId);
        public Task<Response<IEnumerable<TicketResponse>>> GetTicketsByAnalystIdAsync(string analystId);
        public Task<Response<IEnumerable<InteractionResponse>>> GetInteractionsByTicketIdAsync(Guid ticketId);
        public Task<Response<TicketResponse>> AssignTicketTo(CreateAssignTicketRequest request);
        public Task<Response<TicketResponse>> ChangeTicketStatus(ChangeTicketStatusRequest request);
        public Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst);
        public Task<Response<TicketDashboardResponse>> GetDashboardTickets();
    }
}
