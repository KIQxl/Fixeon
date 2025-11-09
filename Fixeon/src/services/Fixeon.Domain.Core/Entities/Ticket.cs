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
            Protocol = this.Id.ToString().Substring(0, 5);
            Description = description;
            Category = category;
            Departament = departament;
            CreateAt = DateTime.Now;
            Status = ETicketStatus.Pending.ToString();
            Priority = priority;
            CreatedByUser = user;

            SLAInfo = new SLAInfo
            {
                FirstInteraction = new SLA(),
                Resolution = new SLA(),
            };
        }

        public string Protocol { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Departament { get; private set; }
        public List<Attachment> Attachments { get; private set; } = new List<Attachment>();
        public User CreatedByUser { get; private set; }
        public Analyst? AssignedTo { get; private set; }
        public DateTime CreateAt { get; private set; }
        public string Status { get; private set; }
        public DateTime? ModifiedAt { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public string Priority { get; private set; }
        public List<Interaction> Interactions { get; private set; } = new List<Interaction>();
        public TimeSpan? Duration => ResolvedAt.HasValue ? ResolvedAt.Value - CreateAt : null;
        public Analyst? ClosedBy { get; private set; }
        public Guid CompanyId { get; private set; }
        public SLAInfo SLAInfo { get; private set; }
        public List<Tag> Tags { get; private set; } = new List<Tag>();

        public bool ResolveTicket(Analyst analyst)
        {
            if (this.Status.Equals(ETicketStatus.InProgress.ToString()) || this.Status.Equals(ETicketStatus.Reopened.ToString()))
            {
                this.Status = ETicketStatus.Resolved.ToString();
                this.ResolvedAt = DateTime.Now;
                this.ModifiedAt = DateTime.Now;
                ClosedBy = analyst;

                SetResolutionAccomplished();

                return true;
            }

            return false;
        }

        public bool AssignTicketToAnalyst(Analyst assignTo)
        {
            if (this.Status == ETicketStatus.Canceled.ToString() || this.Status == ETicketStatus.Resolved.ToString())
                return false;

            this.AssignedTo = assignTo;
            this.Status = ETicketStatus.InProgress.ToString();
            this.ModifiedAt = DateTime.Now;

            SetFirstResponseAccomplished();

            return true;
        }

        public void CancelTicket()
        {
            this.Status = ETicketStatus.Canceled.ToString();
            this.ModifiedAt = DateTime.Now;
        }

        public bool ReOpenTicket()
        {
            if (this.Status != ETicketStatus.Resolved.ToString())
                return false;

            RestartResolutionDate();

            this.Status = ETicketStatus.InProgress.ToString();
            this.ModifiedAt = DateTime.Now;

            return true;
        }

        public bool NewInteraction(Interaction interaction)
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
                return false;

            this.Interactions.Add(interaction);
            this.ModifiedAt = DateTime.Now;

            return true;
        }

        public bool TransferTicket(Analyst assignTo)
        {
            if (this.Status == ETicketStatus.Canceled.ToString())
                return false;

            this.AssignedTo = assignTo;
            this.ModifiedAt = DateTime.Now;

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

        public void ChanceDepartament(string departament)
        {
            this.Departament = departament;
        }

        public void SetFirstInteractionDeadline(int deadlineInMinutes)
            => SLAInfo.SetFirstInteractionDeadline(deadlineInMinutes);

        public void SetResolutionDeadline(int deadlineInMinutes)
            => SLAInfo.SetResolutionDeadline(deadlineInMinutes);

        private void SetFirstResponseAccomplished()
            => SLAInfo.SetFirstResponseAccomplished();

        private void SetResolutionAccomplished()
            => SLAInfo.SetResolutionAccomplished();

        private void RestartResolutionDate()
            => SLAInfo.RestartResolutionDate();
    }
}
