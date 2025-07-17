using Fixeon.Shared.Models;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IBackgroundEmailJobWrapper
    {
        public void SendEmail(EmailMessage email);
    }
}
