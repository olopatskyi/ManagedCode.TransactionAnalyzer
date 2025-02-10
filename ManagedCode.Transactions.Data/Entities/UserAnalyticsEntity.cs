using System.ComponentModel.DataAnnotations.Schema;
using ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;
using ManagedCode.Transactions.Data.Entities.Abstractions;
using ManagedCode.Transactions.Data.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ManagedCode.Transactions.Data.Entities;

[Table("users_analytics")]
public class UserAnalyticsEntity : EntityBase<Guid>
{
    [BsonElement("ti")]
    public decimal TotalIncome { get; set; }

    [BsonElement("te")]
    public decimal TotalExpense { get; set; }

    [BsonElement("tc")]
    public TopCategoryModel TopCategory { get; set; }

    [BsonElement("lat")]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class UserAnalyticsEntityConfiguration(IMongoCollection<UserAnalyticsEntity> collection)
    : EntityConfigurationBase<UserAnalyticsEntity>(collection)
{
    protected override IEnumerable<CreateIndexModel<UserAnalyticsEntity>> IndicesConfiguration()
    {
        var indexKeys = new List<CreateIndexModel<UserAnalyticsEntity>>
        {
            new(
                Builders<UserAnalyticsEntity>.IndexKeys.Descending(a => a.TotalIncome)),

            new(
                Builders<UserAnalyticsEntity>.IndexKeys.Ascending(a => a.TotalExpense)),

            new(
                Builders<UserAnalyticsEntity>.IndexKeys.Descending(a => a.LastUpdated)),

            new(
                Builders<UserAnalyticsEntity>.IndexKeys.Ascending(a => a.TopCategory.Name)),

            new(
                Builders<UserAnalyticsEntity>.IndexKeys.Descending(a => a.TotalIncome)
                    .Descending(a => a.TotalExpense))
        };

        return indexKeys;
    }
}