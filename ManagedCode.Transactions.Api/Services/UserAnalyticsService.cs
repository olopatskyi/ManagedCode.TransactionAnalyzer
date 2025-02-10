using FluentValidation.Results;
using ManagedCode.Transactions.API.Contracts.Requests;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Api.Repositories.Abstractions;
using ManagedCode.Transactions.Api.Services.Abstractions;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using ManagedCode.Transactions.Infrastructure.Services.Abstractions;

namespace ManagedCode.Transactions.API.Services;

public class UserAnalyticsService(IUserAnalyticsRepository repository) : ServiceBase, IUserAnalyticsService
{
    public async Task<ServiceResponse<ReportModel, ValidationResult>> GetReportAsync(GetReportModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var usersSummary = await repository.GetUsersWithHighestIncomeAsync(request, cancellationToken);
        var topCategories = await repository.GetTopTransactionCategoriesAsync(cancellationToken: cancellationToken);
        var highestSpender = await repository.GetHighestSpenderAsync(cancellationToken);

        return Success(new ReportModel
        {
            UsersSummary = usersSummary,
            TopCategories = topCategories,
            HighestSpender = highestSpender
        });
    }
}