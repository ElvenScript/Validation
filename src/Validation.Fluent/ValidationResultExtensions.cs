using FluentValidation;

using Mannaz.Result;

namespace Mannaz.Validation.Fluent;

public static partial class ValidationResultExtensions
{
    public static ValidationResult<T> Validate<T, TValidator>(this ValidationResult<T> result) where TValidator : AbstractValidator<T>
    {
        var validator = Activator.CreateInstance<TValidator>();
        var validationResult = validator.Validate(result.Value);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.ToValidationError();
            return ValidationResult<T>.Failure(error);
        }
        return result;
    }
}
