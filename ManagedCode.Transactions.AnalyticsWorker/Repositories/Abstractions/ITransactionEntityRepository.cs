using ManagedCode.Transactions.AnalyticsWorker.Models;

namespace ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;

public interface ITransactionEntityRepository
{
    Task<List<TransactionModel>> GetManyByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}