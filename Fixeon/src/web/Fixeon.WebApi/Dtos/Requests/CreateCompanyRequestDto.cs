using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Shared.Core.Models;

namespace Fixeon.WebApi.Dtos.Requests
{
    public class CreateCompanyRequestDto
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
        public IFormFile? ProfilePicture { get; set; }

        public CreateCompanyRequest ToApplicationRequest()
        {
            var profilePicture = new FormFileAdapterDto
            {
                FileName = ProfilePicture.FileName,
                ContentType = ProfilePicture.ContentType,
                Length = ProfilePicture.Length,
                Content = ProfilePicture.OpenReadStream()
            };

            var request = new CreateCompanyRequest
            {
                Name = this.Name,
                CNPJ = this.CNPJ,
                Email = this.PhoneNumber,
                PhoneNumber = this.PhoneNumber,
                Street = Street,
                Number = this.Number,
                Neighborhood = this.Neighborhood,
                City = this.City,
                State = this.State,
                PostalCode = this.PostalCode,
                Country = this.Country,
                ProfilePicture = profilePicture
            };

            return request;
        }
    }
}
