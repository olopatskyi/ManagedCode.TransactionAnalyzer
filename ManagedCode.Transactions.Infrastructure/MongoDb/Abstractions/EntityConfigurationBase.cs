using MongoDB.Driver;

namespace ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;

public abstract class EntityConfigurationBase<T>(IMongoCollection<T> collection)
{
    public Task GetMigrationTask()
        => collection.Indexes.CreateManyAsync(
            IndicesConfiguration()
        );

    protected virtual IEnumerable<CreateIndexModel<T>> IndicesConfiguration()
        => new List<CreateIndexModel<T>>();
}