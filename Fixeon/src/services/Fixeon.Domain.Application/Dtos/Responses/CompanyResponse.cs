namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class CompanyResponse
    {
        public CompanyResponse(Guid id, string name, string cNPJ)
        {
            Id = id;
            Name = name;
            CNPJ = cNPJ;
        }

        public CompanyResponse(List<string> errors)
        {
            Errors = errors;
        }

        public CompanyResponse(string error)
        {
            this.Errors.Add(error);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
