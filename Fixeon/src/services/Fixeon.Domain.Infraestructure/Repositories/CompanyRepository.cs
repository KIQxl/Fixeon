using Fixeon.Domain.Application.Interfaces;
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
            return await _context.companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == companyId);
        }
    }
}
