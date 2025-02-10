using ManagedCode.Transactions.Api.Models;

namespace ManagedCode.Transactions.Api.Repositories.Abstractions;

public interface ITransactionsRepository
{
    Task CreateManyAsync(IReadOnlyCollection<TransactionModel> models, CancellationToken cancellationToken = default);
}