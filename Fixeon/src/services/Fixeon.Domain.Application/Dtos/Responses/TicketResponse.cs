using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record TicketResponse
    {
        public Guid Id { get; set; }
        public string Protocol { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public User Customer { get; set; }
        public Analyst Analyst { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Departament { get; set; }
        public List<InteractionResponse>? Interactions { get; set; }
        public SLAInfo SLAInfo { get; set; }
        public string DurationFormat { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public Analyst ClosedBy { get; set; }
        public List<TicketTag> Tags { get; set; }
    }
}
