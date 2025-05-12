using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateInteractionRequest : IRequest
    {
        public Guid TicketId { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string Message { get; set; }
        public string? FirstAttachment { get; set; }
        public string? SecondAttachment { get; set; }
        public string? ThirdAttachment { get; set; }
    }
}
