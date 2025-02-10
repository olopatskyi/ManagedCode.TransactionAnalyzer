using FluentValidation.Results;
using ManagedCode.Transactions.AnalyticsWorker.Models;
using ManagedCode.Transactions.AnalyticsWorker.Models.Requests;
using ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;
using ManagedCode.Transactions.AnalyticsWorker.Services.Abstractions;
using ManagedCode.Transactions.Data.Models;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using ManagedCode.Transactions.Infrastructure.Services.Abstractions;

namespace ManagedCode.Transactions.AnalyticsWorker.Services;

public class UserAnalyticsService(
    ITransactionEntityRepository transactionEntityRepository,
    IUserAnalyticsEntityRepository userAnalyticsEntityRepository) : ServiceBase, IUserAnalyticsService
{
    public async Task<ServiceResponse<ValidationResult>> CreateAnalyticsAsync(CreateAnalyticsModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var transactions =
            await transactionEntityRepository.GetManyByIdsAsync(request.TransactionsIds, cancellationToken);

        var usersAnalytics =
            (await userAnalyticsEntityRepository.GetManyByUsersIdsAsync(transactions.Select(x => x.UserId),
                cancellationToken))
            .ToDictionary(x => x.UserId);

        var updatedAnalytics = GetAggregatedUserAnalytics(transactions, usersAnalytics);
        await userAnalyticsEntityRepository.BulkUpsertAnalyticsAsync(updatedAnalytics.Values.ToList(),
            cancellationToken);

        return Success();
    }

    private static Dictionary<Guid, UserAnalyticsModel> GetAggregatedUserAnalytics(
        List<TransactionModel> transactions,
        Dictionary<Guid, UserAnalyticsModel> usersAnalytics)
    {
        var updatedAnalytics = new Dictionary<Guid, UserAnalyticsModel>();

        foreach (var transaction in transactions)
        {
            var userId = transaction.UserId;

            if (!updatedAnalytics.TryGetValue(userId, out var analytics))
            {
                analytics = usersAnalytics.ContainsKey(userId)
                    ? usersAnalytics[userId]
                    : new UserAnalyticsModel { UserId = userId, CategoryCounts = new Dictionary<string, int>() };

                updatedAnalytics[userId] = analytics;
            }

            if (transaction.Amount > 0)
                analytics.TotalIncome += transaction.Amount;
            else
                analytics.TotalExpense += Math.Abs(transaction.Amount);

            if (analytics.CategoryCounts.ContainsKey(transaction.Category))
                analytics.CategoryCounts[transaction.Category]++;
            else
                analytics.CategoryCounts[transaction.Category] = 1;

            var topCategory = analytics.CategoryCounts
                .OrderByDescending(c => c.Value)
                .FirstOrDefault();

            analytics.TopCategory = new TopCategoryModel
            {
                Name = topCategory.Key ?? "Unknown",
                Count = topCategory.Value
            };

            analytics.LastUpdated = DateTime.UtcNow;
        }

        return updatedAnalytics;
    }
}