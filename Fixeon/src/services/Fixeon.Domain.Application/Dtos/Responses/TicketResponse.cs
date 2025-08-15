namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record TicketResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? OrganizationName { get; set; }
        public string? AssignedTo { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Departament { get; set; }
        public List<InteractionResponse>? Interactions { get; set; }
        public string DurationFormat { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public string? ClosedBy { get; set; }
    }
}
