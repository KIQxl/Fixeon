using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class InteractionResponse
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Attachment Attachments { get; set; }
    }
}
