using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;
using Fixeon.Domain.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DomainContext _context;
        public CompanyRepository(DomainContext dataContext)
        {
            this._context = dataContext;
        }

        public async Task CreateCompany(Company request)
        {
            try
            {
                await _context.companies.AddAsync(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            try
            {
                return await _context.companies.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Company> GetCompanyById(Guid companyId)
        {
            return await _context.companies.AsNoTracking().Include(c => c.Tags).FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<List<Tag>> GetAllTagsByCompany()
        {
            return await _context.tags.AsNoTracking().ToListAsync();
        }

        public async Task<Tag> GetTagById(Guid tagId)
        {
            return await _context.tags.FirstOrDefaultAsync(x => x.Id == tagId);
        }

        public async Task CreateTag(Tag tag)
        {
            await _context.tags.AddAsync(tag);
        }

        // TAGS
        public async Task RemoveTag(Tag tag)
        {
            _context.tags.Remove(tag);
        }
    }
}
