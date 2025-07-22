using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ITicketRepository
    {
        public Task<Ticket> GetTicketByIdAsync(Guid id);
        public Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        public Task<IEnumerable<Ticket>> GetTicketsByCategoryAsync(string category);
        public Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(EPriority priority);
        public Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
        public Task<IEnumerable<Ticket>> GetTicketsByAnalystIdAsync(string analystId);
        public Task<IEnumerable<Ticket>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst);
        public Task CreateTicket(Ticket ticket);
        public Task UpdateTicket(Ticket ticket);

        public Task<TicketAnalysisResponse> GetTicketsAnalysis();
        public Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis();
        public Task<List<TopAnalystResponse>> GetTopAnalyst();

        public Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId);
        public Task CreateInteraction(Interaction interaction);

        public Task CreateAttachment(List<Attachment> attachments);
    }
}
