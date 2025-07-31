using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface IEmailServices
    {
        public Task SendEmail(EmailMessage email);
    }
}
