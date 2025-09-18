using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;

namespace Fixon.Tests.MockRepository
{
    public class FakeNotificationServices : ITicketNotificationServices
    {
        public Task NotifyAnalystTeam(Ticket ticket, Guid companyId)
        {
            throw new NotImplementedException();
        }

        public Task NotifyRequesterAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
