using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
    {
        public CreateTicketRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O campo Título não pode ser vazio.")
                .MinimumLength(8).WithMessage("Tamanho mínimo do Título é 8 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("O campo Descrição não pode ser vazio.")
                .MinimumLength(8).WithMessage("Tamanho mínimo da Descrição é 8 caracteres.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("O campo Categoria não pode ser vazio.");

            RuleFor(x => x.Departament)
                .NotEmpty().WithMessage("O campo Departamento não pode ser vazio.");

            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("O campo Prioridade não pode ser vazio.");

            RuleFor(x => x.Attachments.Count)
                .LessThanOrEqualTo(3).WithMessage("O limite máximo de arquivos é 3.");
        }
    }
}
