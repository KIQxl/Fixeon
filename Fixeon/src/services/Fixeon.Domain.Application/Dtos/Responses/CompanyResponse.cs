using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;
using System.Text.Json.Serialization;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class CompanyResponse
    {
        public CompanyResponse(Guid id, string name, string cnpj, string email, Address address, string phoneNumber, EActiveStatus status, DateTime createdAt, List<Tag>? tags = null, List<OrganizationResponse>? organizations = null)
        {
            Id = id;
            Name = name;
            CNPJ = cnpj;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            Status = status;
            CreatedAt = createdAt;
            Tags = tags ?? new List<Tag>();
            Organizations = organizations ?? new List<OrganizationResponse>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EActiveStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Tag> Tags { get; set; }
        public List<OrganizationResponse> Organizations { get; set; }
    }
}
