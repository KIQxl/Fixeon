using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public record CreateTicketRequest : IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CreateByUserId { get; set; }
        public string CreateByUsername { get; set; }
        public EPriority Priority { get; set; }
        public string? FirstAttachment { get; set; }
        public string? SecondAttachment { get; set; }
        public string? ThirdAttachment { get; set; }
    }
}
