namespace Fixeon.Domain.Core.Entities
{
    public class Attachment : Entity
    {
        private Attachment() { }
        public Attachment(string name, string extension, Guid senderId, Guid? ticketId, Guid? interactionId)
        {
            Name = name;
            Extension = extension;
            SenderId = senderId;
            TicketId = ticketId;
            InteractionId = interactionId;
        }

        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public Guid SenderId { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? InteractionId { get; set; }

        public virtual Ticket? Ticket { get; set; }
        public virtual Interaction? Interaction { get; set; }
    }
}
