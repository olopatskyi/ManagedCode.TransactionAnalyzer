using ManagedCode.Transactions.Api.Contracts.Requests;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ManagedCode.Transactions.Api.Services.Abstractions;

public interface ITransactionsService
{
    Task<ServiceResponse<ValidationResult>> ProcessTransactionsAsync(ProcessTransactionsModelRequest request,
        CancellationToken cancellationToken = default);
}