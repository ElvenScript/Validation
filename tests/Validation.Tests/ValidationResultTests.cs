using Mannaz.Error;
using Mannaz.Result;

namespace Mannaz.Validation.Tests;

public class ValidationResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Arrange
        var value = "TestValue";

        // Act
        var result = ValidationResult<string>.Success(value, State.Ok);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Equal(State.Ok, result.State);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Arrange
        var error = new ValidationError(new Error.Error("ERR001", "Test error"));

        // Act
        var result = ValidationResult<string>.Failure(error, State.Error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Equal(State.Error, result.State);
    }

    [Fact]
    public void Value_ShouldThrowException_WhenResultIsFailure()
    {
        // Arrange
        var result = ValidationResult<string>.Failure(new ValidationError(new Error.Error("ERR001", "Test error")));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void Error_ShouldThrowException_WhenResultIsSuccess()
    {
        // Arrange
        var result = ValidationResult<string>.Success("TestValue");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void WithState_ShouldUpdateStateCorrectly()
    {
        // Arrange
        var result = ValidationResult<string>.Success("TestValue");

        // Act
        var updatedResult = result.WithState(State.Created);

        // Assert
        Assert.Equal(State.Created, updatedResult.State);
    }

    [Fact]
    public void And_ShouldCombineErrorsFromFailedResults()
    {
        // Arrange
        var error1 = new ValidationError(new Error.Error("ERR001", "First error"));
        var error2 = new ValidationError(new Error.Error("ERR002", "Second error"));
        var result1 = ValidationResult<string>.Failure(error1);
        var result2 = ValidationResult<string>.Failure(error2);

        // Act
        var combinedResult = result1.And(result2);

        // Assert
        Assert.False(combinedResult.IsSuccess);
        Assert.Contains(error1.Errors, e => e.Code == "ERR001");
        Assert.Contains(error2.Errors, e => e.Code == "ERR002");
        Assert.Equal(State.Error, combinedResult.State);
    }

    [Fact]
    public void ToProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var result = ValidationResult<string>.Failure(new ValidationError(new Error.Error("ERR001", "Test error")), State.Invalid);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(State.Invalid.ToTitle(), problemDetails.Title);
        Assert.Equal(State.Invalid.ToHttpStatusCode(), problemDetails.Status);
        Assert.Equal(State.Invalid.ToDescription(), problemDetails.Detail);
    }
}