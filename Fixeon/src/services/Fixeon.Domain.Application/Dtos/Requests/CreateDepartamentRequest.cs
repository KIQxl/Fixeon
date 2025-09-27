using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateDepartamentRequest : IRequest
    {
        public Guid OrganizationId { get; set; }
        public string DepartamentName { get; set; }
    }
}
