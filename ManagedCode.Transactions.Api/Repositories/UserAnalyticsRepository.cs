using ManagedCode.Transactions.API.Contracts.Requests;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Api.Repositories.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManagedCode.Transactions.API.Repositories;

public class UserAnalyticsRepository(TransactionAnalyzerContext context) : IUserAnalyticsRepository
{
    private readonly IMongoCollection<UserAnalyticsEntity> _collection = context.UsersAnalytics;

    public async Task<List<UserAnalyticsReportSummaryModel>> GetUsersWithHighestIncomeAsync(GetReportModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var usersSummary = await _collection
            .Find(_ => true)
            .SortByDescending(u => u.TotalIncome)
            .Skip(request.Skip)
            .Limit(request.Limit)
            .Project(x=> new UserAnalyticsReportSummaryModel
            {
                UserId = x.Id.ToString(),
                TotalIncome = x.TotalIncome,
                TotalExpense = x.TotalExpense
            })
            .ToListAsync(cancellationToken);

        return usersSummary;
    }

    public async Task<List<UserAnalyticsReportCategoryModel>> GetTopTransactionCategoriesAsync(int limit = 3, CancellationToken cancellationToken = default)
    {
        var aggregationPipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$tc.n" },
                { "transactions_count", new BsonDocument("$sum", "$tc.c") }
            }),
            new BsonDocument("$sort", new BsonDocument("transactions_count", -1)),
            new BsonDocument("$limit", limit)
        };

        var cursor = await _collection.AggregateAsync<BsonDocument>(aggregationPipeline, cancellationToken: cancellationToken);
        var results = await cursor.ToListAsync(cancellationToken);

        return results.Select(doc => new UserAnalyticsReportCategoryModel
        {
            Category = doc.Contains("_id") && doc["_id"].BsonType == BsonType.String ? doc["_id"].AsString : "Unknown",
            TransactionsCount = doc.Contains("transactions_count") && doc["transactions_count"].BsonType == BsonType.Int32
                ? doc["transactions_count"].AsInt32
                : 0
        }).ToList();
    }

    public async Task<UserAnalyticsReportSpenderModel> GetHighestSpenderAsync(
        CancellationToken cancellationToken = default)
    {
        var highestSpender = await _collection
            .Find(_ => true)
            .SortBy(a => a.TotalExpense)
            .FirstOrDefaultAsync(cancellationToken);

        if (highestSpender == null)
            return null;

        return new UserAnalyticsReportSpenderModel
        {
            UserId = highestSpender.Id.ToString(),
            TotalExpense = highestSpender.TotalExpense
        };
    }
}