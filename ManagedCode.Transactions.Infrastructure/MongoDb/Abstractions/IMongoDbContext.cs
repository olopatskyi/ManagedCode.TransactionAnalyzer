using MongoDB.Driver;

namespace ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }

    Task ApplyMigrationAsync();
}