namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class ChangeTicketCategoryAndDepartament
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DepartamentId { get; set; }
    }
}
