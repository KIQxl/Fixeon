namespace Fixeon.Auth.Infraestructure.Entities
{
    public class Company
    {
        private Company() { }
        public Company(string name, string cNPJ)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cNPJ;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
