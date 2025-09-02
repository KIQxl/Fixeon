using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public struct CreateSLARequest : IRequest
    {
        public string Organization { get; private set; }
        public Guid OrganizationId { get; private set; }
        public int SLAInMinutes { get; private set; }
        public EPriority SLAPriority { get; private set; }
        public ESLAType Type { get; private set; }
    }
}
