namespace Fixeon.Auth.Infraestructure.Entities
{
    public class Organization
    {
        public Organization(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<ApplicationUser> Users { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }
    }
}
