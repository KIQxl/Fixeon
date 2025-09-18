using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.ValueObjects;
using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Domain.Application.Mapper
{
    public static class InteractionMapper
    {
        public static Interaction ToEntity(CreateInteractionRequest request, ITenantContextServices tenantContext)
        {
            return new Interaction(
                request.TicketId,
                request.Message,
                new InteractionUser { UserId = tenantContext.UserId.ToString(), UserEmail = tenantContext.UserEmail }
            );
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
    }
}
