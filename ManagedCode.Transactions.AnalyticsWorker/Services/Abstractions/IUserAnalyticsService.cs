using FluentValidation.Results;
using ManagedCode.Transactions.AnalyticsWorker.Models.Requests;
using ManagedCode.Transactions.Infrastructure.Api.Responses;

namespace ManagedCode.Transactions.AnalyticsWorker.Services.Abstractions;

public interface IUserAnalyticsService
{
    Task<ServiceResponse<ValidationResult>> CreateAnalyticsAsync(CreateAnalyticsModelRequest request,
        CancellationToken cancellationToken = default);
}