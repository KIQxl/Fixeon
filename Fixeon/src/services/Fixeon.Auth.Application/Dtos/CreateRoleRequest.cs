using System.ComponentModel.DataAnnotations;

namespace Fixeon.Auth.Application.Dtos
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string RoleName { get; set; }
    }
}
