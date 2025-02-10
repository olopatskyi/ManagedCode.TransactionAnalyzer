using System.Net;
using FluentValidation.Results;

namespace ManagedCode.Transactions.Infrastructure.Api.Responses;

public class ServiceResponse<TErrorRepresentation>(
    bool isSuccess,
    HttpStatusCode status = HttpStatusCode.OK,
    TErrorRepresentation errors = default)
{
    public bool IsSuccess { get; set; } = isSuccess;

    public HttpStatusCode Status { get; set; } = status;

    public TErrorRepresentation Errors { get; set; } = errors;
}

public class ServiceResponse<TResult, TErrorRepresentation>(
    TResult result,
    bool isSuccess,
    HttpStatusCode status = HttpStatusCode.OK,
    TErrorRepresentation errors = default)
    : ServiceResponse<TErrorRepresentation>(isSuccess, status, errors)
{
    public TResult Result { get; set; } = result;
}