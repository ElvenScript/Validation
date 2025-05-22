# ElvenScript.Validation

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

ElvenScript.Validation is a .NET 8 library for functional and fluent validation, error handling, and result composition. It provides a robust, composable, and testable way to handle validation and error propagation in C# applications, including ASP.NET Core Minimal APIs. The solution consists of three projects:

- **Validation**: Core validation and result types.
- **Validation.Fluent**: Extensions for integration with [FluentValidation](https://fluentvalidation.net/).
- **Validation.Tests**: Unit tests for the validation logic.

## Features

- Functional-style `ValidationResult<T>` for success/failure flows
- Composable error handling with `ValidationError` and `ExtendedError`
- Minimal API integration for ASP.NET Core
- FluentValidation support for seamless rule validation
- Async/await support for validation chains

## Getting Started

### Installation

Add the NuGet package to your project:
# Core validation
Install-Package ElvenScript.Validation -Version x.y.z
# FluentValidation integration
Install-Package ElvenScript.Validation.Fluent -Version x.y.z
### Basic Usage

#### Creating Validation Results
```csharp
using ElvenScript.Result;

var success = ValidationResult<string>.Success("Valid value");
var error = new ValidationError(new Error.Error("ERR001", "Invalid value"));
var failure = ValidationResult<string>.Failure(error);
#### Composing Results
var result1 = ValidationResult<string>.Success("A");
var result2 = ValidationResult<string>.Failure(new ValidationError(new Error.Error("ERR002", "Error B")));
var combined = result1.And(result2); // Combines errors if any
#### Pattern Matching
var output = result1.Match(
    value => $"Success: {value}",
    err => $"Failure: {string.Join(", ", err.Errors.Select(e => e.Description))}"
);
```
#### Minimal API Integration
```csharp
app.MapPost("/validate", (MyModel model) =>
{
    var result = ValidateModel(model);
    return result.ToHttpResult<MyModel, ValidationResult<MyModel>>();
});
```
### FluentValidation Integration
```csharp
using ElvenScript.Validation.Fluent;
using FluentValidation;

public class MyModelValidator : AbstractValidator<MyModel>
{
    public MyModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

var result = ValidationResult<MyModel>.Success(new MyModel { Name = "" })
    .Validate<MyModel, MyModelValidator>();

if (!result.IsSuccess)
{
    // Handle validation errors
}
```
## Testing

Unit tests are provided in the `Validation.Tests` project. Run them using your preferred test runner:
dotnet test
## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements and bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.