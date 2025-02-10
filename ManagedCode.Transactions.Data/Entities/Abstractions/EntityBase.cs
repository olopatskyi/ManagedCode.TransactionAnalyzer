using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagedCode.Transactions.Data.Entities.Abstractions;

public abstract class EntityBase<TIdentifier>
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public TIdentifier Id { get; set; }
}