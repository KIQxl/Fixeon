using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Entities
{
    public class Company : Entity
    {
        protected Company() { }
        public Company(string name, string cNPJ, string email, List<Tag>? tags)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cNPJ;
            Email = email;
            if (tags != null)
                Tags = tags;
        }

        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<Organization> Organizations { get; private set; }
        public List<Tag> Tags { get; private set; } = new List<Tag>();

        public void AddTags(List<Tag> tags)
        {
            if (tags.Any())
            {
                foreach(var tag in tags)
                {
                    this.Tags.Add(tag);
                }
            }
        }
    }
}
