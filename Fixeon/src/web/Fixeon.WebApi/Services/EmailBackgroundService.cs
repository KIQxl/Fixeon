using Fixeon.Shared.Interfaces;

namespace Fixeon.WebApi.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueueServices _queueService;
        private readonly IEmailServices _emailServices;
        private readonly ILogger _logger;

        public EmailBackgroundService(IEmailQueueServices queueService, IEmailServices services, ILogger<EmailBackgroundService> logger)
        {
            _queueService = queueService;
            _emailServices = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _queueService.DequeueEmailAsync();

                if(message != null)
                {
                    try
                    {
                        await _emailServices.SendEmail(message.To, message.Subject, message.Body);
                        _logger.LogInformation($"Email enviado com sucesso para {message.To}.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erro ao enviar email.");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }
    }
}
