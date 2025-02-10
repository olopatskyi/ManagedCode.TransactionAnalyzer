using System.Net;
using FluentValidation.Results;
using ManagedCode.Transactions.Infrastructure.Api.Extensions;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ManagedCode.Transactions.Infrastructure.Api;

public abstract class HttpController : ControllerBase
{
    protected static IActionResult ActionResult(ServiceResponse<ValidationResult> serviceResponse)
    {
        return serviceResponse.IsSuccess
            ? new StatusCodeResult((int)HttpStatusCode.NoContent)
            : ErrorResult(serviceResponse);
    }
    
    protected static IActionResult ActionResult<T>(ServiceResponse<T, ValidationResult> serviceResponse, 
        HttpStatusCode successStatusCode = HttpStatusCode.OK)
    {
        if (!serviceResponse.IsSuccess) return ErrorResult(serviceResponse);
        
        if (successStatusCode is HttpStatusCode.NoContent)
            return new StatusCodeResult((int)successStatusCode);
        
        return new ObjectResult(new ApiResponse<T>(serviceResponse.Result))
        {
            StatusCode = (int)successStatusCode
        };
    }
    
    private static IActionResult ErrorResult(ServiceResponse<ValidationResult> serviceResponse) 
        => new ObjectResult(serviceResponse.Errors.ToApiResponse())
        {
            StatusCode = (int)serviceResponse.Status
        };
}