using FluentValidation;
using FluentValidation.Results;

using Lukdrasil.Result;

using Validation.Error;

namespace Validation.Fluent;

public static partial class ValidationResultExtensions
{
    public static ValidationResult<T> Validate<T, TValidator>(this ValidationResult<T> result) where TValidator : AbstractValidator<T>
    {
        TValidator validator = Activator.CreateInstance<TValidator>();
        ValidationResult validationResult = validator.Validate(result.Value);
        if (!validationResult.IsValid)
        {
            ValidationError error = validationResult.Errors.ToValidationError();
            return ValidationResult<T>.Failure(error);
        }
        return result;
    }
}
