namespace Fixeon.Domain.Application.Contracts
{
    public interface IAuthACL
    {
        public Task<string> GetCompanyEmail(Guid companyId);
    }
}
