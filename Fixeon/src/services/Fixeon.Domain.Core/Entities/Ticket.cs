using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Core.Entities
{
    public class Ticket : Entity
    {
        private Ticket() { }
        public Ticket(string title, string description, string category, User createdByUser, EPriority priority)
        {
            Title = title;
            Description = description;
            Category = category;
            CreatedByUser = createdByUser;
            CreateAt = DateTime.UtcNow;
            Status = ETicketStatus.Pending;
            Priority = priority;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public List<Attachment> Attachments {  get; private set; } = new List<Attachment>();
        public User CreatedByUser { get; private set; }
        public Analist? AssignedTo { get; private set; }
        public DateTime CreateAt { get; private set; }
        public ETicketStatus Status { get; private set; }
        public DateTime? ModifiedAt { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public EPriority Priority { get; private set; }
        public List<Interaction> Interactions { get; private set; } = new List<Interaction>();
        public TimeSpan? Duration => ResolvedAt.HasValue ? ResolvedAt.Value - CreateAt : null;

        public void ResolveTicket()
        {
            this.Status = ETicketStatus.Resolved;
            this.ResolvedAt = DateTime.UtcNow;
        }

        public bool AssignTicketToAnalist(Analist assignTo)
        {
            if (this.Status == ETicketStatus.Canceled)
                return false;

            this.AssignedTo = assignTo;
            this.Status = ETicketStatus.InProgress;
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public void CancelTicket()
        {
            this.Status = ETicketStatus.Canceled;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public bool ReOpenTicket()
        {
            if (this.Status == ETicketStatus.Canceled)
                return false;

            this.Status = ETicketStatus.Reopened;
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public bool NewInteraction(Interaction interaction)
        {
            if (this.Status == ETicketStatus.Canceled)
                return false;

            this.Interactions.Add(interaction);
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public bool TransferTicket(Analist assignTo)
        {
            if (this.Status == ETicketStatus.Canceled)
                return false;

            this.AssignedTo = assignTo;
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public List<Interaction> ListInteractions()
        {
            return this.Interactions;
        }

        public bool AddAttachment(Attachment attachments)
        {
            if (Attachments.Count > 3)
                return false;

            this.Attachments.Add(attachments);
            return true;
        }
    }
}
