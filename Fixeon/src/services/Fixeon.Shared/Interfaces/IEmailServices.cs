using Fixeon.Shared.Models;

namespace Fixeon.Shared.Interfaces
{
    public interface IEmailServices
    {
        public Task SendEmail(EmailMessage email);
    }
}
