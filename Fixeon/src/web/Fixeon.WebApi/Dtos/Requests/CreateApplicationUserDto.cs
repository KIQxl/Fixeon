using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Shared.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Fixeon.WebApi.Dtos.Requests
{
    public class CreateApplicationUserDto
    {
        [Required(ErrorMessage = "O campo Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Username obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo Telefone obrigatório")]
        public string PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public IFormFile? ProfilePictureUrl { get; set; }

        [Required(ErrorMessage = "O campo Senha obrigatório")]
        [MinLength(8, ErrorMessage = "A senha precisa conter pelo menos 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirmação de senha obrigatório")]
        [Compare("Password", ErrorMessage = "Senhas não conferem")]
        public string PasswordConfirm { get; set; }
        public List<string>? Roles { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? OrganizationId { get; set; }

        public CreateAccountRequest ToApplicationRequest()
        {
            var profilePicture = new FormFileAdapterDto
            {
                FileName = ProfilePictureUrl.FileName,
                ContentType = ProfilePictureUrl.ContentType,
                Length = ProfilePictureUrl.Length,
                Content = ProfilePictureUrl.OpenReadStream()
            };

            var request = new CreateAccountRequest
            {
                Email = this.Email,
                Username = this.Username,
                PhoneNumber = this.PhoneNumber,
                JobTitle = this.JobTitle,
                ProfilePictureUrl = profilePicture,
                Password = this.Password,
                PasswordConfirm = this.PasswordConfirm,
                Roles = this.Roles,
                CompanyId = this.CompanyId,
                OrganizationId = this.OrganizationId
            };

            return request;
        }
    }
}
