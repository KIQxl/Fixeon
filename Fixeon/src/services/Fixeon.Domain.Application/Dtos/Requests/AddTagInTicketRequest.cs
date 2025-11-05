namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class AddTagInTicketRequest
    {
        public Guid TicketId { get; set; }
        public Guid TagId { get; set; }
    }
}
