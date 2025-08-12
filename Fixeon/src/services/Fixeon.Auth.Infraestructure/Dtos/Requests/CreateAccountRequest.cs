using System.ComponentModel.DataAnnotations;

namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public record CreateAccountRequest
    {
        [Required(ErrorMessage = "O campo Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Username obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo Senha obrigatório")]
        [MinLength(8, ErrorMessage = "A senha precisa conter pelo menos 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirmação de senha obrigatório")]
        [Compare("Password", ErrorMessage = "Senhas não conferem")]
        public string PasswordConfirm { get; set; }

        public Guid? CompanyId { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
