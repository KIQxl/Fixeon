namespace Fixeon.Auth.Core.Entities
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
