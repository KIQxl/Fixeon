using Fixeon.Domain.Application.Dtos.Requests;
using FluentValidation;

namespace Fixeon.Domain.Application.Validations
{
    class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
                .NotNull().WithMessage("Nome da categoria é obrigatório.");
        }
    }
}
