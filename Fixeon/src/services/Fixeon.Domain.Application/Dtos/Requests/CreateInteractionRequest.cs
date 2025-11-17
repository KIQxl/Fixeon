using Fixeon.Domain.Application.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateInteractionRequest : IRequest
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public List<FormFileAdapterDto> Attachments { get; set; } = new List<FormFileAdapterDto>();
    }
}
