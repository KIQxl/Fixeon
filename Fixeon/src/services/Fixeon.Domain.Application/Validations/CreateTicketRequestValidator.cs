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

            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("O campo Prioridade não pode ser vazio.")
                .IsInEnum().WithMessage("Valor inválido para Prioridade.");

            RuleFor(x => x.CreateByUserId)
                .NotEmpty().WithMessage("Código do usuário de criação não pode ser vazio.")
                .Length(36).WithMessage("Código do usuário de criação inválido.");

            RuleFor(x => x.CreateByUsername)
                .NotEmpty().WithMessage("O campo usuário de criação não pode ser vazio.");
        }
    }
}
