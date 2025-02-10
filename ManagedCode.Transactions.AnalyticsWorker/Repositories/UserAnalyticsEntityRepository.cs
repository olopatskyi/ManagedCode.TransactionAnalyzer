using AutoMapper;
using ManagedCode.Transactions.AnalyticsWorker.Models;
using ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Data.Entities;
using MongoDB.Driver;

namespace ManagedCode.Transactions.AnalyticsWorker.Repositories;

public class UserAnalyticsEntityRepository(TransactionAnalyzerContext context, IMapper mapper)
    : IUserAnalyticsEntityRepository
{
    private readonly IMongoCollection<UserAnalyticsEntity> _collection = context.UsersAnalytics;

    public async Task BulkUpsertAnalyticsAsync(List<UserAnalyticsModel> models,
        CancellationToken cancellationToken = default)
    {
        var analyticsList = mapper.Map<List<UserAnalyticsEntity>>(models);

        var bulkOperations = new List<WriteModel<UserAnalyticsEntity>>();

        foreach (var analytics in analyticsList)
        {
            var filter = Builders<UserAnalyticsEntity>.Filter.Eq(x => x.Id, analytics.Id);
            var update = new ReplaceOneModel<UserAnalyticsEntity>(filter, analytics) { IsUpsert = true };
            bulkOperations.Add(update);
        }

        if (bulkOperations.Count > 0)
        {
            await _collection.BulkWriteAsync(bulkOperations, cancellationToken: cancellationToken);
        }
    }

    public Task<List<UserAnalyticsModel>> GetManyByUsersIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserAnalyticsEntity>.Filter.In(x => x.Id, userIds);
        return _collection.Find(filter)
            .Project(x=>new UserAnalyticsModel
            {
                UserId = x.Id,
                TotalExpense = x.TotalExpense,
                TotalIncome = x.TotalIncome
            })
            .ToListAsync(cancellationToken);
    }
}