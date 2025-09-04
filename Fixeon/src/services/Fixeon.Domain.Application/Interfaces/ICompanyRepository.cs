using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ICompanyRepository
    {
        public Task CreateCompany(Company request);
        public Task<List<Company>> GetAllCompanies();
        public Task<Company> GetCompanyById(Guid companyId);
    }
}
