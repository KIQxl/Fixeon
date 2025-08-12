namespace Fixeon.Auth.Infraestructure.Entities
{
    public class Company
    {
        protected Company() { }
        public Company(string name, string cNPJ)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cNPJ;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<ApplicationUser> Users { get; private set; }
        public List<Organization> Organizations { get; private set; }
    }
}
