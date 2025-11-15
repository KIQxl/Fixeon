using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Entities
{
    public class Company : Entity
    {
        protected Company() { }
        public Company(string name, string cNPJ, string email, string phoneNumber, Address address, List<Tag>? tags)
        {
            Id = Guid.NewGuid();
            Name = name;
            CNPJ = cNPJ;
            Email = email;
            CreatedAt = DateTime.Now;
            Status = EActiveStatus.Trial;
            Tags = tags ?? new List<Tag>();
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public EActiveStatus Status { get; private set; }
        public List<Tag> Tags { get; private set; } = new List<Tag>();
        public List<Organization> Organizations { get; private set; }

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
