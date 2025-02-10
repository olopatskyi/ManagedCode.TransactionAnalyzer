using ManagedCode.Transactions.AnalyticsWorker.Models;
using ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Data.Entities;
using MongoDB.Driver;

namespace ManagedCode.Transactions.AnalyticsWorker.Repositories;

public class TransactionEntityRepository(TransactionAnalyzerContext context) : ITransactionEntityRepository
{
    private readonly IMongoCollection<TransactionEntity> _collection = context.Transactions;

    public Task<List<TransactionModel>> GetManyByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<TransactionEntity>.Filter.In(x => x.Id, ids);
        return _collection.Find(filter)
            .Project(x => new TransactionModel
            {
                UserId = x.UserId,
                Amount = x.Amount,
                Category = x.Category
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}