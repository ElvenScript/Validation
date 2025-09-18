using Lukdrasil.Error;
using Lukdrasil.Result;

using Microsoft.AspNetCore.Mvc;

using Validation.Error;

namespace Validation.Tests;

public class ValidationResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Arrange
        string value = "TestValue";

        // Act
        ValidationResult<string> result = ValidationResult<string>.Success(value, State.Ok);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Equal(State.Ok, result.State);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Arrange
        ValidationError error = new ValidationError(new Lukdrasil.Error.Error("ERR001", ErrorSeverity.Error, "Test error"));

        // Act
        ValidationResult<string> result = ValidationResult<string>.Failure(error, State.Error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Equal(State.Error, result.State);
    }

    [Fact]
    public void Value_ShouldThrowException_WhenResultIsFailure()
    {
        // Arrange
        ValidationResult<string> result = ValidationResult<string>.Failure(new ValidationError(new Lukdrasil.Error.Error("ERR001", ErrorSeverity.Error, "Test error")));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void Error_ShouldThrowException_WhenResultIsSuccess()
    {
        // Arrange
        ValidationResult<string> result = ValidationResult<string>.Success("TestValue");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void WithState_ShouldUpdateStateCorrectly()
    {
        // Arrange
        ValidationResult<string> result = ValidationResult<string>.Success("TestValue");

        // Act
        ValidationResult<string> updatedResult = result.WithState(State.Created);

        // Assert
        Assert.Equal(State.Created, updatedResult.State);
    }

    [Fact]
    public void And_ShouldCombineErrorsFromFailedResults()
    {
        // Arrange
        ValidationError error1 = new ValidationError(new Lukdrasil.Error.Error("ERR001", ErrorSeverity.Error, "First error"));
        ValidationError error2 = new ValidationError(new Lukdrasil.Error.Error("ERR002", ErrorSeverity.Error, "Second error"));
        ValidationResult<string> result1 = ValidationResult<string>.Failure(error1);
        ValidationResult<string> result2 = ValidationResult<string>.Failure(error2);

        // Act
        ValidationResult<string> combinedResult = result1.And(result2);

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
        ValidationResult<string> result = ValidationResult<string>.Failure(new ValidationError(new Lukdrasil.Error.Error("ERR001", ErrorSeverity.Error, "Test error")), State.Invalid);

        // Act
        ProblemDetails problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(State.Invalid.ToTitle(), problemDetails.Title);
        Assert.Equal(State.Invalid.ToHttpStatusCode(), problemDetails.Status);
        Assert.Equal(State.Invalid.ToDescription(), problemDetails.Detail);
    }
}