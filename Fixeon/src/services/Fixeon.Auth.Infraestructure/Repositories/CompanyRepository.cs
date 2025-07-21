using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;
        public CompanyRepository(DataContext dataContext)
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
    }
}
