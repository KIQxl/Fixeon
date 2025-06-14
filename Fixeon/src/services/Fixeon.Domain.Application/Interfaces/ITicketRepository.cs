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
        public Task<IEnumerable<Ticket>> GetTicketsByAnalistIdAsync(string analistId);
        public Task<bool> CreateTicket(Ticket ticket);
        public Task<bool> UpdateTicket(Ticket ticket);

        public Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId);
        public Task CreateInteraction(Interaction interaction);

        public Task<bool> CreateAttachment(List<Attachment> attachments);
    }
}
