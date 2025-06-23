namespace Fixeon.Domain.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public Task<bool> Commit();
    }
}
