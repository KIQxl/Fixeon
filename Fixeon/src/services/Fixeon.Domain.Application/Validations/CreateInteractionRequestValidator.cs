using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    public class CreateInteractionRequestValidator : AbstractValidator<CreateInteractionRequest>
    {
        public CreateInteractionRequestValidator()
        {
            RuleFor(x => x.CreatedByUserId)
                .NotEmpty().WithMessage("Código do usuário de criação não pode ser nulo.")
                .Length(36).WithMessage("Código do usuário de criação inválido.");

            RuleFor(x => x.CreatedByUserName)
                .NotEmpty().WithMessage("Nome do usuário de criação não pode ser nulo.");

            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("Código do ticket inválido.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Mensagem não pode ser vazia.")
                .MinimumLength(8).WithMessage("Mensagem deve conter no mínimo 8 caracteres.");

            RuleFor(x => x.Attachments.Count)
                .LessThanOrEqualTo(3).WithMessage("O limite máximo de arquivos é 3.");
        }
    }
}
