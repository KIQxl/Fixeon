using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface IBackgroundEmailJobWrapper
    {
        public void SendEmail(EmailMessage email);
    }
}
