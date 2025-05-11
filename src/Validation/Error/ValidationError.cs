using System.Collections;
namespace Mannaz.Error;

public record ValidationError() : IEnumerable<BaseError>
{
    public HashSet<BaseError> Errors { get; init; } = [];
    public ValidationError(BaseError error) : this()
    {
        Errors = [error];
    }

    public ValidationError(IEnumerable<BaseError> errors) : this()
    {
        Errors = errors?.ToHashSet() ?? [];
    }

    public ValidationError Add(BaseError error) => this with { Errors = [.. Errors, error] };
    public ValidationError Add(ValidationError error) => this with { Errors = [.. Errors, .. error] };

    public IEnumerator<BaseError> GetEnumerator()
    {
        return Errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Errors.GetEnumerator();
    }
}
