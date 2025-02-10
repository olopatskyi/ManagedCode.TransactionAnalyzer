using FluentValidation.Results;
using ManagedCode.Transactions.API.Contracts.Requests;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Infrastructure.Api.Responses;

namespace ManagedCode.Transactions.Api.Services.Abstractions;

public interface IUserAnalyticsService
{
    Task<ServiceResponse<ReportModel, ValidationResult>> GetReportAsync(GetReportModelRequest request,
        CancellationToken cancellationToken = default);
}