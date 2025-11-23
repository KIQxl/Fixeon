using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Shared.Core.Models;

namespace Fixeon.WebApi.Dtos.Requests
{
    public class CreateOrganizationRequestDto
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public IFormFile? ProfilePictureUrl { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Departaments { get; set; } = new List<string>();
        public List<CreateSLAInOrganizationRequest> Slas { get; set; } = new List<CreateSLAInOrganizationRequest>();

        public CreateOrganizationRequest ToApplicationRequest()
        {
            var profilePicture = ProfilePictureUrl != null ? new FormFileAdapterDto
            {
                FileName = ProfilePictureUrl.FileName,
                ContentType = ProfilePictureUrl.ContentType,
                Length = ProfilePictureUrl.Length,
                Content = ProfilePictureUrl.OpenReadStream()
            }
            : null;

            var request = new CreateOrganizationRequest
            {
                Name = this.Name,
                CNPJ = this.CNPJ,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Notes = this.Notes,
                Street = Street,
                Number = this.Number,
                Neighborhood = this.Neighborhood,
                City = this.City,
                State = this.State,
                PostalCode = this.PostalCode,
                Country = this.Country,
                ProfilePictureUrl = profilePicture,
                Categories = this.Categories,
                Departaments = this.Departaments,
                Slas = this.Slas
            };

            return request;
        }
    }
}
