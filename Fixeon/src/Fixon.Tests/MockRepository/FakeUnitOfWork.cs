using Fixeon.Domain.Application.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public async Task<bool> Commit()
        {
            return true;
        }
    }
}
