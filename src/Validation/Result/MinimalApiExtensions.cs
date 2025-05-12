using ElvenScript.Error;

using Microsoft.AspNetCore.Http;

namespace ElvenScript.Result;

public static partial class MinimalApiExtensions
{
    public static IResult ToHttpResult<T, TResult>(this TResult result, string? createdUri = null) where TResult : ValidationResult<T>
    {
        return result.State switch
        {
            State.Unknown => throw new NotSupportedException($"Result {result.State} conversion is not supported."),
            State.Ok => TypedResults.Ok(),
            State.OkWithContent => TypedResults.Ok(result.Value),
            State.Created => TypedResults.Created(createdUri),
            State.Error or State.Unavailable or State.CriticalError => TypedResults.Problem(result.ToProblemDetails()),
            State.Forbidden => TypedResults.Forbid(),
            State.Unauthorized => TypedResults.Unauthorized(),
            State.NotFound => TypedResults.NotFound(),
            State.NoContent => TypedResults.NoContent(),
            State.Invalid => TypedResults.ValidationProblem(result.ToErrorDictionary<T, TResult>()),
            _ => TypedResults.StatusCode(result.State.ToHttpStatusCode()),

        };

    }

    private static Dictionary<string, string[]> ToErrorDictionary<T, TResult>(this TResult result) where TResult : ValidationResult<T>
    {
        if (result is ValidationResult<T> validationResult && validationResult.Error is ValidationError errors)
        {
            return errors.Cast<ExtendedError>()
                .GroupBy(e => e.FieldName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );
        }
        return [];
    }
}

