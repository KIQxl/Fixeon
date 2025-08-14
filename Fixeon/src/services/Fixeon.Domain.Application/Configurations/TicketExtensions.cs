using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Fixeon.Domain.Application.Configurations
{
    public static class TicketExtensions
    {
        public static TicketResponse ToResponse(this Ticket ticket, List<string>? attachmentsUrls = null)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreateAt,
                ModifiedAt = ticket.ModifiedAt,
                ResolvedAt = ticket.ResolvedAt,
                CreatedBy = ticket.CreatedByUser.UserEmail,
                OrganizationName = ticket.CreatedByUser.OrganizationName,
                AssignedTo = ticket.AssignedTo?.AnalystName,
                Category = ticket.Category,
                Departament = ticket.Departament,
                Interactions = ticket.Interactions.Select(i => i.ToInteractionResponse()).ToList(),
                Priority = ticket.Priority,
                Status = ticket.Status,
                DurationFormat = ticket.Duration.HasValue ? $"{(int)ticket.Duration.Value.TotalDays} dias, {(int)ticket.Duration.Value.Hours} horas e {(int)ticket.Duration.Value.Minutes} minutos" : "Em análise",
                Duration = ticket.Duration,
                Attachments = attachmentsUrls
            };
        }

        public static void AddInteractionsResponseForTicket(this TicketResponse ticketResponse, List<InteractionResponse>? interactions = null)
        {
            ticketResponse.Interactions = interactions;
        }

        public static InteractionResponse ToInteractionResponse(this Interaction interaction, List<string>? attachmentsUrls = null)
        {
            return new InteractionResponse
            {
                Message = interaction.Message,
                CreatedByUserId = interaction.CreatedBy.UserId,
                CreatedByUserName = interaction.CreatedBy.UserEmail,
                TicketId = interaction.TicketId,
                CreatedAt = interaction.CreatedAt,
                AttachmentsUrls = attachmentsUrls
            };
        }

        public static ValidationResult Validate<T>(this T obj) where T : IRequest
        {
            var validatorType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.BaseType?.IsGenericType == true &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) &&
                    t.BaseType.GenericTypeArguments[0] == typeof(T));

            if (validatorType == null)
                return null;

            var validator = (IValidator<T>)Activator.CreateInstance(validatorType)!;

            return validator.Validate(obj);
        }
    }
}
