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
        }
    }
}
