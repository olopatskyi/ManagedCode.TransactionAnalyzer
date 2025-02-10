using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;

public abstract class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }

    protected MongoDbContext(IOptions<MongoDbContextOptions> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        
        var client = new MongoClient(options.Value.ConnectionString);
        Database = client.GetDatabase(options.Value.Database);
        InitializeCollections();
    }

    private void InitializeCollections()
    {
        MethodInfo getCollectionMethod = typeof(IMongoDatabase).GetMethod("GetCollection");

        foreach (var propertyInfo in GetType().GetProperties()
                     .Where(p => p.PropertyType.IsGenericType &&
                                 p.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>)))
        {
            if (getCollectionMethod == null)
            {
                throw new NullReferenceException("GetCollection method not found");
            }

            if (!propertyInfo.CanWrite) continue;

            var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];

            // **Check for [Table("name")] attribute**
            var tableAttribute = genericArgument.GetCustomAttribute<TableAttribute>();
            var collectionName = tableAttribute != null ? tableAttribute.Name : genericArgument.Name.ToLower() + "s";

            var genericMethod = getCollectionMethod.MakeGenericMethod(genericArgument);
            var collection = genericMethod.Invoke(Database, new object[] { collectionName, null });

            propertyInfo.SetValue(this, collection);
            Console.WriteLine($"{propertyInfo.Name} => {collectionName}");
        }
    }

    public async Task ApplyMigrationAsync()
    {
        Console.WriteLine("Applying migration...");
        await MigrateAsync();
        Console.WriteLine("Migration applied successfully");
    }

    public virtual Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
