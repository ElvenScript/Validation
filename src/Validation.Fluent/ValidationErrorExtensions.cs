using FluentValidation.Results;

using Lukdrasil.Error;

using Validation.Error;

namespace Validation.Fluent;

public static partial class ValidationErrorExtensions
{
    public static ValidationError ToValidationError(this FluentValidation.Results.ValidationFailure validationFailure)
    {
        Lukdrasil.Error.Error error = new Lukdrasil.Error.Error(validationFailure.ErrorCode, validationFailure.Severity switch
        {
            FluentValidation.Severity.Warning => ErrorSeverity.Warning,
            FluentValidation.Severity.Info => ErrorSeverity.Info,
            _ => ErrorSeverity.Error

        }, validationFailure.ErrorMessage);
        return new ValidationError(error);
    }
    public static ValidationError ToValidationError(this List<FluentValidation.Results.ValidationFailure> validationFailures)
    {
        ValidationError error = [];
        foreach (ValidationFailure validationFailure in validationFailures)
        {
            error = error.Add(validationFailure.ToValidationError());
        }
        return error;
    }
}