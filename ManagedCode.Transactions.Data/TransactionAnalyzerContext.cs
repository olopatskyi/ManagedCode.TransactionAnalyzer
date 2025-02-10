using ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;
using ManagedCode.Transactions.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ManagedCode.Transactions.Data;

public class TransactionAnalyzerContext(IOptions<MongoDbContextOptions> options) : MongoDbContext(options)
{
    public IMongoCollection<TransactionEntity> Transactions { get; set; }
    
    public IMongoCollection<UserAnalyticsEntity> UsersAnalytics { get; set; }
    
    public override async Task MigrateAsync()
    {
        await new TransactionEntityConfiguration(Transactions).GetMigrationTask();
        await new UserAnalyticsEntityConfiguration(UsersAnalytics).GetMigrationTask();
    }
}