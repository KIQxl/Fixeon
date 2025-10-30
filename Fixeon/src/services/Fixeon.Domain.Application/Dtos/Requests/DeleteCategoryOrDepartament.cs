namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class DeleteCategoryOrDepartament
    {
        public Guid OrganizationId { get; set; }
        public Guid CategoryOrDepartamentId { get; set; }
    }
}
