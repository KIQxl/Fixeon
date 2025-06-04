using Fixeon.Auth.Application.Interfaces;
using Fixeon.Shared.Interfaces;
using Fixeon.Shared.Models;
using Hangfire;

namespace Fixeon.WebApi.Services
{
    public class HangfireWrapper : IBackgroundEmailJobWrapper
    {
        private readonly IEmailServices _emailServices;

        public HangfireWrapper(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        public void SendWelcomeEmail(EmailMessage email)
        {
            BackgroundJob.Enqueue(() => 
                _emailServices.SendEmail(email.To, email.Subject, email.Body));
        }
    }
}
