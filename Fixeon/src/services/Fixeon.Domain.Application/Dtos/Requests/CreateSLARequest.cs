using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateSLARequest : IRequest
    {
        public Guid OrganizationId { get; set; }
        public int SLAInMinutes { get; set; }
        public EPriority SLAPriority { get; set; }
        public ESLAType Type { get; set; }
    }
}
