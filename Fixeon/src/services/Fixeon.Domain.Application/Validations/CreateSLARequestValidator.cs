using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    public class CreateSLARequestValidator : AbstractValidator<CreateSLARequest>
    {
        public CreateSLARequestValidator()
        {
            RuleFor(x => x.SLAInMinutes)
                .GreaterThan(0).WithMessage("O tempo em minutos da SLA deve ser maior que zero.")
                .NotNull().WithMessage("O tempo em minutos da SLA não pode ser nulo.");

            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Código de organização inválido.");

            RuleFor(x => x.SLAPriority)
                .NotEmpty().WithMessage("O campo Prioridade da SLA não pode ser vazio"); ;

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O campo de tipo da SLA não pode ser vazio");
        }
    }
}
