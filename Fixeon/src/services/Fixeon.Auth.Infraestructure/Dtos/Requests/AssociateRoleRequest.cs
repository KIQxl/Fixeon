using System.ComponentModel.DataAnnotations;

namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public class AssociateRoleRequest
    {
        [Required(ErrorMessage = "O campo id do Usuário é obrigatório")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string RoleName { get; set; }
    }
}
