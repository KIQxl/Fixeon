namespace Fixeon.Auth.Infraestructure.Dtos.Requests
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
    }
}
