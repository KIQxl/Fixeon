using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Core.Entities
{
    public class Interaction : Entity
    {
        private Interaction() { }
        public Interaction(Guid ticketId, string message, InteractionUser user)
        {
            TicketId = ticketId;
            Message = message;
            CreatedBy = user;
            CreatedAt = DateTime.Now;
        }

        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public DateTime CreatedAt { get; set; }
        public InteractionUser CreatedBy { get; set; }

        public virtual Ticket Ticket { get; set; }

        public bool AddAttachment(Attachment attachments)
        {
            if (Attachments.Count > 3)
                return false;

            this.Attachments.Add(attachments);
            return true;
        }
    }
}
