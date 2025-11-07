using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.ValueObjects;
using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Domain.Application.Mapper
{
    public static class TicketMapper
    {
        public static Ticket ToEntity(CreateTicketRequest request, User customer)
        {
            return new Ticket(
                request.Title,
                request.Description,
                request.Category,
                request.Departament,
                request.Priority.ToString(),
                customer
            );
        }

        public static TicketResponse ToResponse(this Ticket ticket, List<string>? attachmentsUrls = null)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                Protocol = ticket.Protocol,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreateAt,
                ModifiedAt = ticket.ModifiedAt,
                ResolvedAt = ticket.ResolvedAt,
                Customer = ticket.CreatedByUser,
                Analyst = ticket.AssignedTo ?? new Analyst(),
                Category = ticket.Category,
                Departament = ticket.Departament,
                Interactions = ticket.Interactions.Select(i => i.ToInteractionResponse()).ToList(),
                Priority = ticket.Priority,
                Status = ticket.Status,
                DurationFormat = ticket.Duration.HasValue ? $"{(int)ticket.Duration.Value.TotalDays} dias, {(int)ticket.Duration.Value.Hours} horas e {(int)ticket.Duration.Value.Minutes} minutos" : "Em análise",
                Duration = ticket.Duration,
                Attachments = attachmentsUrls,
                ClosedBy = ticket.ClosedBy ?? new Analyst(),
                SLAInfo = ticket.SLAInfo,
                Tags = ticket.Tags.Select(t => new TicketTag { Id = t.Id, Name = t.Name }).ToList()
            };
        }

        public static void AddInteractionsResponseForTicket(this TicketResponse ticketResponse, List<InteractionResponse>? interactions = null)
        {
            ticketResponse.Interactions = interactions;
        }
    }
}
