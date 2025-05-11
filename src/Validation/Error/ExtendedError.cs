using Microsoft.AspNetCore.Mvc;
namespace Mannaz.Error;

public record ExtendedError(string Code, string Description, string FieldName, ErrorSeverity Severity = ErrorSeverity.Error)
    : Error(Code, Description, Severity)
{
    public override ProblemDetails ToProblemDetails()
    {
        return new ProblemDetails
        {
            Title = Code,
            Detail = Description,
            Status = (int)Severity,
            Extensions = { ["FieldName"] = FieldName }
        };
    }
}