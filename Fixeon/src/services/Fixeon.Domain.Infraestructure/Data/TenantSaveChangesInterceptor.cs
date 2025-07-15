using Fixeon.Domain.Core.Interfaces;
using Fixeon.Shared.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fixeon.Domain.Infraestructure.Data
{
    public class TenantSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ITenantContext _tenantContext;

        public TenantSaveChangesInterceptor(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var tenantId = _tenantContext.TenantId;

            var context = eventData.Context;

            if(context != null)
            {
                foreach(var entry in context.ChangeTracker.Entries()
                    .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added && e.Entity is ITenantEntity))
                {
                    entry.Property("CompanyId").CurrentValue = tenantId;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
