using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    class CreateDepartamentValidator : AbstractValidator<CreateDepartamentRequest>
    {
        public CreateDepartamentValidator()
        {
            RuleFor(c => c.DepartamentName)
                .NotEmpty().WithMessage("Nome da departamento é obrigatório.")
                .NotNull().WithMessage("Nome da departamento é obrigatório.");
        }
    }
}
