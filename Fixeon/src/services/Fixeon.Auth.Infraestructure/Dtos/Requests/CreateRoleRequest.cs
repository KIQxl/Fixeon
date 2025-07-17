using System.ComponentModel.DataAnnotations;

namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string RoleName { get; set; }
    }
}
