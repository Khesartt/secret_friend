using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace secretFriend.Api.Domain.Entities;

public class WishlistItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonRequired]
    public string GameId { get; set; } = string.Empty;

    [BsonRequired]
    public string PlayerEmail { get; set; } = string.Empty;

    [BsonRequired]
    public string ProductName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? ApproximateValue { get; set; }

    public List<string> WebsiteLinks { get; set; } = [];

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? UpdatedAt { get; set; }
}