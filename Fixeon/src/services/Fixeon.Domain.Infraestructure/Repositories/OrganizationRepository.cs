using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Entities;
using Fixeon.Domain.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Fixeon.Domain.Infraestructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DomainContext _context;

        public OrganizationRepository(DomainContext context)
        {
            _context = context;
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            try
            {
                return await _context.organizations.AsNoTracking().Include(o => o.SLAs).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Organization> GetOrganizationById(Guid organizationId)
        {
            try
            {
                return await _context.organizations.Include(o => o.SLAs).FirstOrDefaultAsync(x => x.Id == organizationId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Organization>> GetOrganizations(IEnumerable<Guid> organizationIds)
        {
            return await _context.organizations
                .Where(o => organizationIds.Contains(o.Id))
                .ToListAsync();
        }


        public async Task CreateOrganization(Organization organization)
        {
            try
            {
                await _context.organizations.AddAsync(organization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateOrganization(Organization organization)
        {
            try
            {
                _context.Attach(organization);
                _context.Entry(organization).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task DeleteOrganization(Organization organization)
        {
            try
            {
                _context.organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // SLA
        public async Task<List<OrganizationsSLA>> GetSLAByOrganization(Guid organizationId)
        {
            try
            {
                return await _context.organizationsSLAs
                    .AsNoTracking()
                    .Where(x => x.OrganizationId == organizationId)
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
                await _context.organizationsSLAs.AddAsync(organizationSLA);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateSLA(OrganizationsSLA organizationSLA)
        {
            try
            {
                _context.Attach(organizationSLA);
                _context.Entry(organizationSLA).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }
    }
}
