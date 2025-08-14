namespace Fixeon.Domain.Application.Dtos.Requests
{
    public record CreateAssignTicketRequest
    {
        public Guid TicketId { get; set; }
        public string AnalystId { get; set; }
        public string AnalystEmail { get; set; }
    }
}
