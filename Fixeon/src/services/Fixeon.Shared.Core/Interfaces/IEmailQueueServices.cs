using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface IEmailQueueServices
    {
        public Task EnqueueEmailAsync(EmailMessage message);
        public Task<EmailMessage?> DequeueEmailAsync();
    }
}
