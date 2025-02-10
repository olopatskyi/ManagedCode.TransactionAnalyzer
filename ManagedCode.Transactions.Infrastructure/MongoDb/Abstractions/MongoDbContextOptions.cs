namespace ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;

public class MongoDbContextOptions
{
    public string ConnectionString { get; set; }
    
    public string Database { get; set; }
}