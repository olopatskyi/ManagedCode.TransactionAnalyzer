using FluentValidation;
using FluentValidation.Results;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using Microsoft.Extensions.Localization;

namespace ManagedCode.Transactions.Infrastructure.Api.Extensions;

public static class ValidationExtensions
{
    public static ApiResponse<object> ToApiResponse(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToList()
            );

        return new ApiResponse<object>(400, "Validation failed", errors);
    }
    
    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder,
        IStringLocalizer localizer = null, string message = "The property is required.")
        => ruleBuilder
            .NotEmpty()
            .WithMessage(message);
}