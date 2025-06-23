using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Infraestructure.Data;

namespace Fixeon.Domain.Infraestructure.Repositories
{
    public class UnitOfWOrk : IUnitOfWork
    {
        private readonly DomainContext _ctx;

        public UnitOfWOrk(DomainContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Commit()
        {
            try
            {
                var result = await _ctx.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível realizar a operação. {ex.Message}");
            }
        }
    }
}
