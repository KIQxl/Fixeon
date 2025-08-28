using Fixeon.Domain.Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Fixeon.Domain.Application.Validator
{
    public static class EntityValidator
    {
        public static ValidationResult Validate<T>(this T obj) where T : IRequest
        {
            var validatorType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.BaseType?.IsGenericType == true &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) &&
                    t.BaseType.GenericTypeArguments[0] == typeof(T));

            if (validatorType == null)
                return null;

            var validator = (IValidator<T>)Activator.CreateInstance(validatorType)!;

            return validator.Validate(obj);
        }
    }
}
