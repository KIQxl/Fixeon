using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface ICompanyResolver
    {
        public Task<CompanyResolverView> GetCompany(Guid companyId);
    }
}
