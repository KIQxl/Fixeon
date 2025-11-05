using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ICompanyRepository
    {
        public Task CreateCompany(Company request);
        public Task<List<Company>> GetAllCompanies();
        public Task<Company> GetCompanyById(Guid companyId);

        // TAGS
        public Task<List<Tag>> GetAllTagsByCompany();
        public Task<Tag> GetTagById(Guid tagId);
        public Task CreateTag(Tag tag);
        public Task RemoveTag(Tag tag);
    }
}
