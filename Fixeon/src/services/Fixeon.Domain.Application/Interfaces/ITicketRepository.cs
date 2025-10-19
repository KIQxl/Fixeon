using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ITicketRepository
    {
        public Task<Ticket> GetTicketByIdAsync(Guid id);
        public Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        public Task<IEnumerable<Ticket>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user, string? protocol);
        public Task CreateTicket(Ticket ticket);
        public Task UpdateTicket(Ticket ticket);

        public Task<TicketAnalysisResponse> GetTicketsAnalysis();
        public Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis();
        public Task<List<TopAnalystResponse>> GetTopAnalyst();
        public Task<List<TicketsByHourResponse>> GetTicketsByHourAsync();
        public Task<List<TicketsByDayResponse>> GetTicketsByDayAsync();

        public Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId);
        public Task CreateInteraction(Interaction interaction);

        public Task CreateAttachment(List<Attachment> attachments);
    }
}
