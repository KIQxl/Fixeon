namespace Fixeon.Domain.Application.Dtos.Requests
{
    public record CreateAssignTicketRequest
    {
        public Guid TicketId { get; set; }
        public string AnalistId { get; set; }
        public string AnalistName { get; set; }
    }
}
