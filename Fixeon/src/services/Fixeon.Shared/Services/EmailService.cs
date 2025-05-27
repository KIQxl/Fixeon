using Fixeon.Shared.Interfaces;

namespace Fixeon.Shared.Services
{
    public class EmailService : IEmailServices
    {
        public Task SendEmail(string to, string subject, string body)
        {
            var test = "a";

            throw new NotImplementedException();
        }
    }
}
