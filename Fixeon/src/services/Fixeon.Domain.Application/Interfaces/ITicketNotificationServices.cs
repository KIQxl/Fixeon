using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ITicketNotificationServices
    {
        public Task NotifyRequesterAsync(string email);
        public Task NotifyAnalystTeam(Ticket ticket, Guid companyId);
    }
}
