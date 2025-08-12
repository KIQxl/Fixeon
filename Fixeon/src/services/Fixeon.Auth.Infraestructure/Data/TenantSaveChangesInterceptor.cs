using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fixeon.Auth.Infraestructure.Data
{
    public class TenantSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ITenantContext _tenantContext;
        public TenantSaveChangesInterceptor(ITenantContext tenantContext)
        {
            this._tenantContext = tenantContext;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context as DataContext;

            if (context == null || context.IgnoreTenantInterceptor)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var tenantId = _tenantContext.TenantId;

            if (context != null)
            {
                foreach (var entry in context.ChangeTracker.Entries()
                    .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added && e.Entity is ITenantEntity))
                {
                    entry.Property("CompanyId").CurrentValue = tenantId;
                }

                foreach (var entry in context.ChangeTracker.Entries()
                    .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added && e.Entity is Organization))
                {
                    entry.Property("CompanyId").CurrentValue = tenantId;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
