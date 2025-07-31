using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;

namespace Fixeon.Shared.Services
{
    public class HangfireWrapper : IBackgroundEmailJobWrapper
    {
        private readonly IEmailServices _emailServices;

        public HangfireWrapper(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        public void SendEmail(EmailMessage email)
        {
            var client = new BackgroundJobClient();

            client.Create(
                Job.FromExpression(() => _emailServices.SendEmail(email)),
                new EnqueuedState("email")
            );
        }

    }
}
