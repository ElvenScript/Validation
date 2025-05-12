using ElvenScript.Error;
using ElvenScript.Result;

namespace ElvenScript.Result;

public static partial class ValidationResultExtensions
{
    public static ValidationResult<T2> Map<T1, T2>(this ValidationResult<T1> result, Func<T1, T2> map) =>
        result.IsSuccess
            ? ValidationResult<T2>.Success(map(result.Value))
            : ValidationResult<T2>.Failure(result.Error);


    public static ValidationResult<T2> Bind<T1, T2>(
        this ValidationResult<T1> result, Func<T1, ValidationResult<T2>> bind) =>
        result.IsSuccess
            ? bind(result.Value)
            : ValidationResult<T2>.Failure(result.Error);


    public static TResult Match<T, TResult>(this ValidationResult<T> result, Func<T, TResult> mapValue, Func<ValidationError, TResult> mapError) =>
        result.IsSuccess
            ? mapValue(result.Value)
            : mapError(result.Error);

    public static async Task<ValidationResult<T2>> MapAsync<T1, T2>(
        this Task<ValidationResult<T1>> result, Func<T1, T2> map) =>
            await result.ContinueWith(t => t.Result.Map(map));


    public static async Task<ValidationResult<T2>> MapAsync<T1, T2>(
        this ValidationResult<T1> result, Func<T1, Task<T2>> mapAsync) =>
        result.IsSuccess ? ValidationResult<T2>.Success(await mapAsync(result.Value))
                         : ValidationResult<T2>.Failure(result.Error);


    public static async Task<ValidationResult<T2>> MapAsync<T1, T2>(
        this Task<ValidationResult<T1>> result, Func<T1, Task<T2>> mapAsync) =>
        await (await result).MapAsync(mapAsync);


    public static async Task<ValidationResult<T2>> BindAsync<T1, T2>(
        this Task<ValidationResult<T1>> result, Func<T1, ValidationResult<T2>> bind) =>
            await result.ContinueWith(t => t.Result.Bind(bind));

    public static async Task<ValidationResult<T2>> BindAsync<T1, T2>(
        this ValidationResult<T1> result, Func<T1, Task<ValidationResult<T2>>> bindAsync) =>
        result.IsSuccess ? await bindAsync(result.Value)
                         : ValidationResult<T2>.Failure(result.Error);


    public static async Task<ValidationResult<T2>> BindAsync<T1, T2>(
        this Task<ValidationResult<T1>> result, Func<T1, Task<ValidationResult<T2>>> bindAsync) =>
        await (await result).BindAsync(bindAsync);


    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<ValidationResult<T>> result, Func<T, TResult> onSuccess, Func<ValidationError, TResult> onFailure) =>
        (await result).Match(onSuccess, onFailure);


    public static async Task<TResult> MatchAsync<T, TResult>(
        this ValidationResult<T> result, Func<T, Task<TResult>> onSuccessAsync, Func<ValidationError, TResult> onFailure) =>
        result.IsSuccess ? await onSuccessAsync(result.Value)
                         : onFailure(result.Error);

    public static async Task<TResult> MatchAsync<T, TResult>(
        this ValidationResult<T> result, Func<T, TResult> onSuccess, Func<ValidationError, Task<TResult>> onFailureAsync) =>
        result.IsSuccess ? onSuccess(result.Value)
                         : await onFailureAsync(result.Error);


    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<ValidationResult<T>> result, Func<T, Task<TResult>> onSuccessAsync, Func<ValidationError, TResult> onFailure) =>
        await (await result).MatchAsync(onSuccessAsync, onFailure);

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<ValidationResult<T>> result, Func<T, TResult> onSuccess, Func<ValidationError, Task<TResult>> onFailureAsync) =>
        await (await result).MatchAsync(onSuccess, onFailureAsync);


    public static async Task<TResult> MatchAsync<T, TResult>(
        this ValidationResult<T> result, Func<T, Task<TResult>> onSuccessAsync, Func<ValidationError, Task<TResult>> onFailureAsync) =>
        result.IsSuccess ? await onSuccessAsync(result.Value)
                         : await onFailureAsync(result.Error);

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<ValidationResult<T>> result, Func<T, Task<TResult>> onSuccessAsync, Func<ValidationError, Task<TResult>> onFailureAsync) =>
        await (await result).MatchAsync(onSuccessAsync, onFailureAsync);


}
