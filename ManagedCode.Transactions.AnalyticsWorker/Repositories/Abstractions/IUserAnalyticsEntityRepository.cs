using ManagedCode.Transactions.AnalyticsWorker.Models;

namespace ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;

public interface IUserAnalyticsEntityRepository
{
    Task BulkUpsertAnalyticsAsync(List<UserAnalyticsModel> analyticsList,
        CancellationToken cancellationToken = default);

    Task<List<UserAnalyticsModel>> GetManyByUsersIdsAsync(IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
}