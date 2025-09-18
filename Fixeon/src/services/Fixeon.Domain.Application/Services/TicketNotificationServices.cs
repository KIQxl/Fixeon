using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Services
{
    public class TicketNotificationServices : ITicketNotificationServices
    {
        private readonly IBackgroundEmailJobWrapper _backgroundServices;
        private readonly ICompanyResolver _companyResolver;

        public TicketNotificationServices(IBackgroundEmailJobWrapper backgroundServices, ICompanyResolver companyResolver)
        {
            _backgroundServices = backgroundServices;
            _companyResolver = companyResolver;
        }

        public Task NotifyRequesterAsync(string email)
        {
            _backgroundServices.SendEmail(new EmailMessage
            {
                To = email,
                Subject = "Ticket registrado com sucesso!",
                Body = EmailDictionary.ConfirmationTicketOpening
            });

            return Task.CompletedTask;
        }

        public async Task NotifyAnalystTeam(Ticket ticket, Guid companyId)
        {
            var company = await _companyResolver.GetCompany(companyId);

            _backgroundServices.SendEmail(new EmailMessage
            {
                To = company.CompanyEmail,
                Subject = "Novo ticket aberto",
                Body = EmailDictionary.NewTicketInformAnalysts
                    .Replace("{ticketId}", ticket.Id.ToString())
                    .Replace("{ticketUser}", ticket.CreatedByUser.UserEmail)
                    .Replace("{ticketTitle}", ticket.Title)
                    .Replace("{ticketCreatedAt}", ticket.CreateAt.ToString("dd/MM/yyyy HH:mm"))
            });
        }
    }
}
