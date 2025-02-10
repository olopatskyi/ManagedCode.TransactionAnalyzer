using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.API.Contracts.Requests;

namespace ManagedCode.Transactions.Api.Repositories.Abstractions;

public interface IUserAnalyticsRepository
{
    Task<List<UserAnalyticsReportSummaryModel>> GetUsersWithHighestIncomeAsync(GetReportModelRequest request,
        CancellationToken cancellationToken = default);

    Task<List<UserAnalyticsReportCategoryModel>> GetTopTransactionCategoriesAsync(int limit = 3,
        CancellationToken cancellationToken = default);

    Task<UserAnalyticsReportSpenderModel> GetHighestSpenderAsync(
        CancellationToken cancellationToken = default);
}