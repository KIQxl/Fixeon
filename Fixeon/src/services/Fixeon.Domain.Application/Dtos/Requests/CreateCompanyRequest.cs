using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateCompanyRequest : IRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
    }
}
