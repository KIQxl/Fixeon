using Fixeon.Domain.Application.Interfaces;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Contracts
{
    public class CompanyResolver : ICompanyResolver
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyResolver(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResolverView> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyById(companyId);

            if (company is null)
                return new CompanyResolverView();

            return new CompanyResolverView
            {
                CompanyId = companyId,
                CompanyName = company.Name,
                CompanyEmail = company.Email
            };
        }
    }
}
