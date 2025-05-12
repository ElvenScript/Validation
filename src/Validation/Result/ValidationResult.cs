using ElvenScript.Error;

using Microsoft.AspNetCore.Mvc;

namespace ElvenScript.Result;

public record ValidationResult<T> : IApiResult<T, ValidationError>
{
    private T? _value;
    private ValidationError _error = new();

    public bool IsSuccess { get; }

    public T Value
    {
        get => IsSuccess ? _value! : throw new InvalidOperationException("ValidationResult is not successful");
        private set => _value = value;
    }

    public ValidationError Error
    {
        get => !IsSuccess ? _error : throw new InvalidOperationException("ValidationResult is successful");
        private set => _error = value;
    }
    public State State { get; private set; } = State.Unknown;

    private ValidationResult(bool isSuccess, T? value, ValidationError error, State state) => (IsSuccess, Value, Error, State) = (isSuccess, value, error, state);

    public ValidationResult<T> WithState(State state) => this with { State = state };

    public static ValidationResult<T> Success(T value, State state = State.Ok) => new ValidationResult<T>(true, value, default, state);
    public static ValidationResult<T> Failure(ValidationError error, State state = State.Error) => new ValidationResult<T>(false, default, error, state);

    public ValidationResult<T> And(ValidationResult<T> nextResult)
    {
        if (!nextResult.IsSuccess)
        {
            _error = _error.Add(nextResult.Error);
            State = State.Error;
        }
        return this;
    }

    public ProblemDetails ToProblemDetails() => new ProblemDetails
    {
        Title = State.ToTitle(),
        Status = State.ToHttpStatusCode(),
        Detail = State.ToDescription()
    };

}