namespace Fixeon.Auth.Application.Dtos.Responses
{
    public class CompanyResponse
    {
        public CompanyResponse(Guid id, string name, string cNPJ)
        {
            Id = id;
            Name = name;
            CNPJ = cNPJ;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
    }
}
