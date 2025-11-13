using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Fixeon.Shared.Services
{
    public class EmailService : IEmailServices
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailService(IOptions<SmtpSettings> smtpSettings, IHttpClientFactory httpClientFactory)
        {
            _smtpSettings = smtpSettings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendEmail(EmailMessage email)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _smtpSettings.RequestSendEmailUrl);

                request.Headers.Add("api-key", _smtpSettings.ApiKey);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var emailPayload = new
                {
                    sender = new
                    {
                        name = _smtpSettings.SenderName,
                        email = _smtpSettings.SenderEmail
                    },
                    to = new[] { new { email = email.To } },
                    subject = email.Subject,
                    htmlContent = email.Body
                };

                var jsonPayload = JsonSerializer.Serialize(emailPayload);

                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
