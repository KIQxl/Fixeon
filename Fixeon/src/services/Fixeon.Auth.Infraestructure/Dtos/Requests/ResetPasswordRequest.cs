using System.ComponentModel.DataAnnotations;

namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public record ResetPasswordRequest
    {
        [Required(ErrorMessage = "O campo de email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo de nova senha é obrigatório.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }
}
