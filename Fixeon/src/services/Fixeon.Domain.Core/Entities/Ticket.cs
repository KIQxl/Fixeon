using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Core.Entities
{
    public class Ticket : Entity, ITenantEntity
    {
        private Ticket() { }
        public Ticket(string title, string description, string category, string departament, string priority, User user)
        {
            Title = title;
            Description = description;
            Category = category;
            Departament = departament;
            CreateAt = DateTime.UtcNow;
            Status = ETicketStatus.Pending.ToString();
            Priority = priority;
            CreatedByUser = user;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Departament { get; private set; }
        public List<Attachment> Attachments {  get; private set; } = new List<Attachment>();
        public User CreatedByUser { get; private set; }
        public Analyst? AssignedTo { get; private set; }
        public DateTime CreateAt { get; private set; }
        public string Status { get; private set; }
        public DateTime? ModifiedAt { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public string Priority { get; private set; }
        public List<Interaction> Interactions { get; private set; } = new List<Interaction>();
        public TimeSpan? Duration => ResolvedAt.HasValue ? ResolvedAt.Value - CreateAt : null;

        public Guid CompanyId { get; private set; }

        public void ResolveTicket()
        {
            this.Status = ETicketStatus.Resolved.ToString();
            this.ResolvedAt = DateTime.UtcNow;
        }

        public bool AssignTicketToAnalyst(Analyst assignTo)
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
                return false;

            this.AssignedTo = assignTo;
            this.Status = ETicketStatus.InProgress.ToString();
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public void CancelTicket()
        {
            this.Status = ETicketStatus.Canceled.ToString();
            this.ModifiedAt = DateTime.UtcNow;
        }

        public bool ReOpenTicket()
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
                return false;

            this.Status = ETicketStatus.Reopened.ToString();
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public bool NewInteraction(Interaction interaction)
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
                return false;

            this.Interactions.Add(interaction);
            this.ModifiedAt = DateTime.UtcNow;

            return true;
        }

        public bool TransferTicket(Analyst assignTo)
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
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

        public void ChangeCategory(string category)
        {
            this.Category = category;
        }
    }
}
