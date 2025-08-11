using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class InteractionResponse
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> AttachmentsUrls { get; set; }
    }
}
