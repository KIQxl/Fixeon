using Fixeon.Shared.Interfaces;
using Fixeon.Shared.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Fixeon.Shared.Services
{
    public class EmailQueueServices : IEmailQueueServices
    {
        private readonly IDatabase _database;
        private const string QueueKey = "emailQueue";

        public EmailQueueServices(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<EmailMessage?> DequeueEmailAsync()
        {
            var serialized = await _database.ListLeftPopAsync(QueueKey);
            if(serialized.IsNullOrEmpty) 
                return null;

            return JsonSerializer.Deserialize<EmailMessage?>(serialized);
        }

        public async Task EnqueueEmailAsync(EmailMessage message)
        {
            var serialized = JsonSerializer.Serialize(message);
            await _database.ListRightPushAsync(QueueKey, serialized);
        }
    }
}
