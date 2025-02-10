using System.ComponentModel.DataAnnotations.Schema;
using ManagedCode.Transactions.Data.Entities.Abstractions;
using ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ManagedCode.Transactions.Data.Entities;

[Table("transactions")]
public class TransactionEntity : EntityBase<Guid>
{
    [BsonElement("uid")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("dt")]
    public DateTime Date { get; set; }

    [BsonElement("a")]
    public decimal Amount { get; set; }

    [BsonElement("c")]
    public string Category { get; set; }

    [BsonElement("ds")]
    public string Description { get; set; }

    [BsonElement("m")]
    public string Merchant { get; set; }
}

public class TransactionEntityConfiguration(IMongoCollection<TransactionEntity> collection)
    : EntityConfigurationBase<TransactionEntity>(collection)
{
    protected override IEnumerable<CreateIndexModel<TransactionEntity>> IndicesConfiguration()
    {
        var indices = new List<CreateIndexModel<TransactionEntity>>
        {
            // Index on UserId for fast lookups per user
            new CreateIndexModel<TransactionEntity>(
                Builders<TransactionEntity>.IndexKeys.Ascending(t => t.UserId)
            )
        };

        return indices;
    }
}