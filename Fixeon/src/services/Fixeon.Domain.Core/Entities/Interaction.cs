using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Core.Entities
{
    public class Interaction : Entity
    {
        private Interaction() { }
        public Interaction(Guid ticketId, string message, InteractionUser createdBy)
        {
            TicketId = ticketId;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public Attachment Attachments { get; set; }
        public DateTime CreatedAt { get; set; }
        public InteractionUser CreatedBy { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
