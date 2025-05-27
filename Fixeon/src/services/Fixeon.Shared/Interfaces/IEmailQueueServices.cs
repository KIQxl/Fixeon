using Fixeon.Shared.Models;

namespace Fixeon.Shared.Interfaces
{
    public interface IEmailQueueServices
    {
        public Task EnqueueEmailAsync(EmailMessage message);
        public Task<EmailMessage?> DequeueEmailAsync();
    }
}
