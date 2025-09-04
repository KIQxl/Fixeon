using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Entities
{
    public class Company : Entity
    {
        protected Company() { }
        public Company(string name, string cNPJ, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cNPJ;
            Email = email;
        }

        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<Organization> Organizations { get; private set; }
    }
}
