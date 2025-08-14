using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public record CreateTicketRequest : IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Departament { get; set; }
        public EPriority Priority { get; set; }
        public List<FormFileAdapterDto> Attachments { get; set; } = new List<FormFileAdapterDto>();
    }
}
