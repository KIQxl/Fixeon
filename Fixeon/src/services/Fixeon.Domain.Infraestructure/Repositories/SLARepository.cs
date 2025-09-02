using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Repositories
{
    public class SLARepository : ISLARepository
    {
        private readonly DomainContext _ctx;

        public SLARepository(DomainContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<OrganizationsSLA>> GetSLAByOrganizationAndPriority(Guid organizationId, EPriority priority)
        {
            try
            {
                return await _ctx.organizationsSLAs
                    .AsNoTracking()
                    .Where(x => x.SLAPriority == priority.ToString() && x.OrganizationId == organizationId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddOrganizationSLA(OrganizationsSLA organizationSLA)
        {
            try
            {
                await _ctx.organizationsSLAs.AddAsync(organizationSLA);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async void UpdateOrganizationSLA(OrganizationsSLA organizationSLA)
        {
            try
            {
                _ctx.Attach(organizationSLA);
                _ctx.Entry(organizationSLA).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
