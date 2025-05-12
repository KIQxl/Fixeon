using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public record ChangeTicketStatusRequest
    {
        public Guid TicketId { get; set; }
        public ETicketStatus Status { get; set; }
    }
}
