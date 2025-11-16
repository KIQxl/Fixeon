using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    public class CreateOrganizationRequestValidator : AbstractValidator<CreateOrganizationRequest>
    {
        public CreateOrganizationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da organização não pode ser vazio ou nulo.")
                .MinimumLength(2).WithMessage("Mensagem deve conter no mínimo 2 caracteres.");

            RuleFor(x => x.CNPJ)
                .NotEmpty().WithMessage("CNPJ não pode ser vazio ou nulo.")
                .Length(14).WithMessage("Mensagem deve conter no mínimo 14 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email não pode ser vazio ou nulo.")
                .EmailAddress().WithMessage("Formato de email inválido");

            RuleFor(x => x.PhoneNumber)
               .NotNull().WithMessage("Telefone não pode ser nulo.")
               .NotEmpty().WithMessage("Telefone não pode ser vazio.")
               .Length(13).WithMessage("Telefone inválido.");

            RuleFor(x => x.Street)
                .NotNull().WithMessage("Rua não pode ser nulo.")
                .NotEmpty().WithMessage("Rua não pode ser vazio.")
                .MaximumLength(100).WithMessage("Máximo de caracteres para rua: 100")
                .MinimumLength(2).WithMessage("Minimo de caracteres para rua: 2");

            RuleFor(x => x.Number)
                .NotNull().WithMessage("Numero não pode ser nulo.")
                .NotEmpty().WithMessage("Numero não pode ser vazio.")
                .MaximumLength(6).WithMessage("Máximo de caracteres para numero: 6")
                .MinimumLength(1).WithMessage("Mínimo de caracteres para numero: 1");

            RuleFor(x => x.Neighborhood)
                .NotNull().WithMessage("Bairro não pode ser nulo.")
                .NotEmpty().WithMessage("Bairro não pode ser vazio.")
                .MaximumLength(100).WithMessage("Máximo de caracteres para Bairro: 100")
                .MinimumLength(2).WithMessage("Mínimo de caracteres para Bairro: 2");

            RuleFor(x => x.City)
                .NotNull().WithMessage("Cidade não pode ser nulo.")
                .NotEmpty().WithMessage("Cidade não pode ser vazio.")
                .MaximumLength(50).WithMessage("Máximo de caracteres para Cidade: 50")
                .MinimumLength(2).WithMessage("Mínimo de caracteres para Cidade: 2");

            RuleFor(x => x.State)
                .NotNull().WithMessage("Estado não pode ser nulo.")
                .NotEmpty().WithMessage("Estado não pode ser vazio.")
                .MaximumLength(50).WithMessage("Máximo de caracteres para Estado: 50")
                .MinimumLength(2).WithMessage("Mínimo de caracteres para Estado: 2");

            RuleFor(x => x.PostalCode)
                .NotNull().WithMessage("CEP não pode ser nulo.")
                .NotEmpty().WithMessage("CEP não pode ser vazio.")
                .Length(8).WithMessage("CEP inválido.");

            RuleFor(x => x.Country)
                .NotNull().WithMessage("País não pode ser nulo.")
                .NotEmpty().WithMessage("País não pode ser vazio.")
                .MaximumLength(50).WithMessage("Máximo de caracteres para País: 50")
                .MinimumLength(2).WithMessage("Mínimo de caracteres para País: 2");
        }
    }
}
