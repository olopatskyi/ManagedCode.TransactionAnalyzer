using MongoDB.Bson.Serialization.Attributes;

namespace ManagedCode.Transactions.Data.Models;

public class TopCategoryModel
{
    [BsonElement("n")]
    public string Name { get; set; }

    [BsonElement("c")]
    public int Count { get; set; }
}