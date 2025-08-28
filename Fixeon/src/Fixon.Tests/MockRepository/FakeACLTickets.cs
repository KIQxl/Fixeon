using Fixeon.Domain.Application.Contracts;

namespace Fixon.Tests.MockRepository
{
    public class FakeACLTickets : IAuthACL
    {
        public Task<string> GetCompanyEmail(Guid companyId)
        {
            throw new NotImplementedException();
        }
    }
}
