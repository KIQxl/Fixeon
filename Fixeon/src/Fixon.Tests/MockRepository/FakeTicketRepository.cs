using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;

namespace Fixon.Tests.MockRepository
{
    public class FakeTicketRepository : ITicketRepository
    {
        public Task CreateAttachment(List<Attachment> attachments)
        {
            return Task.CompletedTask;
        }

        public Task CreateInteraction(Interaction interaction)
        {
            return Task.CompletedTask;
        }

        public Task CreateTicket(Ticket ticket)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            if (id.ToString().Equals("e7b7f8e4-6d99-4f3e-99aa-f5a7b87b9e71"))
                return new Ticket("Titulo do Ticket", "AAAAAAAAAAAAA", "Geral", "Aeeee", new Fixeon.Domain.Core.ValueObjects.User { UserId = "e7b7f8e4-6d99-4f3e-99aa-f5a7b87b9e71", UserName = "Lojista" }, EPriority.Low.ToString());

            return null;
        }

        public Task<IEnumerable<Ticket>> GetTicketsByAnalistIdAsync(string analistId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(EPriority priority)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTicket(Ticket ticket)
        {
            return Task.CompletedTask;
        }
    }
}
