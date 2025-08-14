using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixon.Tests.MockRepository
{
    public class FakeBackgroundJob : IBackgroundEmailJobWrapper
    {
        public void SendEmail(EmailMessage email)
        {
            // Do nothing :)
        }
    }
}
