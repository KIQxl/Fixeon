using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class OrganizationSLAResponse
    {
        public Guid CompanyId { get; set; }
        public string Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public int SLAInMinutes { get; set; }
        public string SLAPriority { get; set; }
        public ESLAType Type { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
