using Microsoft.AspNetCore.Mvc;
namespace ElvenScript.Error;

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