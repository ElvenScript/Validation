using Lukdrasil.Error;
using Validation.Error;

namespace Validation.Tests;

public class ValidationErrorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyErrorSet()
    {
        ValidationError error = new();
        Assert.Empty(error.Errors);
    }

    [Fact]
    public void SingleErrorConstructor_ShouldContainThatError()
    {
        BaseError baseError = new Lukdrasil.Error.Error("E001", ErrorSeverity.Error, "Message");
        ValidationError error = new(baseError);
        Assert.Single(error.Errors);
        Assert.Contains(baseError, error.Errors);
    }

    [Fact]
    public void EnumerableConstructor_WithNull_ShouldCreateEmptySet()
    {
        IEnumerable<BaseError>? source = null;
        ValidationError error = new(source);
        Assert.Empty(error.Errors);
    }

    [Fact]
    public void EnumerableConstructor_ShouldDeduplicateSameInstance()
    {
        BaseError baseError = new Lukdrasil.Error.Error("E002", ErrorSeverity.Error, "Dup");
        ValidationError error = new(new[] { baseError, baseError });
        Assert.Single(error.Errors);
    }

    [Fact]
    public void AddBaseError_ShouldReturnNewInstance_AndNotMutateOriginal()
    {
        BaseError baseError = new Lukdrasil.Error.Error("E003", ErrorSeverity.Error, "Add");
        ValidationError original = new();

        ValidationError updated = original.Add(baseError);

        Assert.Empty(original.Errors); // immutability
        Assert.Single(updated.Errors);
        Assert.Contains(baseError, updated.Errors);
    }

    [Fact]
    public void AddValidationError_ShouldMergeErrors()
    {
        BaseError e1 = new Lukdrasil.Error.Error("E004", ErrorSeverity.Error, "First");
        BaseError e2 = new Lukdrasil.Error.Error("E005", ErrorSeverity.Error, "Second");
        ValidationError first = new(e1);
        ValidationError second = new(e2);

        ValidationError merged = first.Add(second);

        Assert.Single(first.Errors); // original unchanged
        Assert.Single(second.Errors); // original unchanged
        Assert.Equal(2, merged.Errors.Count);
        Assert.Contains(e1, merged.Errors);
        Assert.Contains(e2, merged.Errors);
    }

    [Fact]
    public void AddValidationError_WithDuplicate_ShouldNotCreateDuplicates()
    {
        BaseError e1 = new Lukdrasil.Error.Error("E006", ErrorSeverity.Error, "Same");
        ValidationError first = new(e1);
        ValidationError second = new(e1);

        ValidationError merged = first.Add(second);

        Assert.Equal(1, merged.Errors.Count);
    }

    [Fact]
    public void Enumeration_ShouldEnumerateAllErrors()
    {
        BaseError e1 = new Lukdrasil.Error.Error("E007", ErrorSeverity.Error, "One");
        BaseError e2 = new Lukdrasil.Error.Error("E008", ErrorSeverity.Error, "Two");
        ValidationError error = new ValidationError(e1).Add(e2);

        List<BaseError> list = [.. error];
        Assert.Equal(2, list.Count);
        Assert.Contains(e1, list);
        Assert.Contains(e2, list);
    }

    [Fact]
    public void AddTwoErrors_FromEmpty_ShouldContainBothAndKeepOriginalEmpty()
    {
        ValidationError empty = new();
        BaseError e1 = new Lukdrasil.Error.Error("E010", ErrorSeverity.Error, "First From Empty");
        BaseError e2 = new Lukdrasil.Error.Error("E011", ErrorSeverity.Error, "Second From Empty");

        ValidationError withFirst = empty.Add(e1);
        ValidationError withBoth = withFirst.Add(e2);

        Assert.Empty(empty.Errors); // original still empty
        Assert.Single(withFirst.Errors);
        Assert.Equal(2, withBoth.Errors.Count);
        Assert.Contains(e1, withBoth.Errors);
        Assert.Contains(e2, withBoth.Errors);
    }

    [Fact]
    public void AddSameErrorTwice_FromEmpty_ShouldNotDuplicate()
    {
        ValidationError empty = new();
        BaseError e1 = new Lukdrasil.Error.Error("E012", ErrorSeverity.Error, "Same Twice");

        ValidationError result = empty.Add(e1).Add(e1);

        Assert.Empty(empty.Errors); // original remains empty
        Assert.Equal(1, result.Errors.Count);
        Assert.Contains(e1, result.Errors);
    }
}
