using System.Net;
using FluentValidation.Results;
using ManagedCode.Transactions.Infrastructure.Api.Responses;

namespace ManagedCode.Transactions.Infrastructure.Services.Abstractions;

public abstract class ServiceBase
{
    protected ServiceResponse<ValidationResult> Success()
    {
        return new ServiceResponse<ValidationResult>(isSuccess: true);
    }
    
    protected ServiceResponse<TResult, ValidationResult> Success<TResult>(TResult result)
    {
        return new ServiceResponse<TResult, ValidationResult>(result: result, isSuccess: true);
    }
    
    protected ServiceResponse<TResult, ValidationResult> Failure<TResult>(TResult result, ValidationResult validationResult)
    {
        return new ServiceResponse<TResult, ValidationResult>(result: result, isSuccess: false, errors: validationResult);
    }
    
    protected ServiceResponse<ValidationResult> Failure(ValidationResult validationResult)
    {
        return new ServiceResponse<ValidationResult>(isSuccess: false, errors: validationResult, status: HttpStatusCode.BadRequest);
    }
}