

using ElvenScript.Error;

namespace ElvenScript.Validation.Fluent;

public static partial class ValidationErrorExtensions
{
    public static ValidationError ToValidationError(this FluentValidation.Results.ValidationFailure validationFailure)
    {
        var error = new Error.Error(validationFailure.ErrorCode, validationFailure.Severity switch
        {
            FluentValidation.Severity.Warning => ErrorSeverity.Warning,
            FluentValidation.Severity.Info => ErrorSeverity.Info,
            _ => ErrorSeverity.Error

        }, validationFailure.ErrorMessage);
        return new ValidationError(error);
    }
    public static ValidationError ToValidationError(this List<FluentValidation.Results.ValidationFailure> validationFailures)
    {
        var error = new ValidationError();
        foreach (var validationFailure in validationFailures)
        {
            error.Add(validationFailure.ToValidationError());
        }
        return error;
    }
}