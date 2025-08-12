namespace Fixeon.Auth.Infraestructure.Entities
{
    public class Organization
    {
        public Organization(string name, Guid companyId)
        {
            Id = Guid.NewGuid();
            Name = name;
            CompanyId = companyId;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<ApplicationUser> Users { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }
    }
}
