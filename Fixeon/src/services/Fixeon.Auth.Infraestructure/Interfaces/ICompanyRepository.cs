using Fixeon.Auth.Infraestructure.Entities;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface ICompanyRepository
    {
        public Task CreateCompany(Company request);
        public Task<List<Company>> GetAllCompanies();
        public Task<Company> GetCompanyById(Guid companyId);
    }
}
