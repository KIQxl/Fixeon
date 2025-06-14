using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateInteractionRequest : IRequest
    {
        public Guid TicketId { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string Message { get; set; }
        public List<FormFileAdapterDto> Attachments { get; set; } = new List<FormFileAdapterDto>();
    }
}
