using MongoDB.Driver;
using secretFriend.Api.Domain.Entities;
using secretFriend.Api.Domain.Repositories;
using secretFriend.Api.Infrastructure.Persistence.MongoDb;

namespace secretFriend.Api.Infrastructure.Persistence.Repositories;

public class WishlistRepository(IMongoDatabase database) : MongoRepositoryBase<WishlistItem>(database, "wishlists"), IWishlistRepository
{
    protected IMongoCollection<WishlistItem> Collection => collection;
    public async Task<IEnumerable<WishlistItem>> GetByGameAndPlayerAsync(string gameId, string playerEmail)
    {
        var filter = Builders<WishlistItem>.Filter.And(
            Builders<WishlistItem>.Filter.Eq(x => x.GameId, gameId),
            Builders<WishlistItem>.Filter.Eq(x => x.PlayerEmail, playerEmail)
        );

        var cursor = await Collection.FindAsync(filter);
        return await cursor.ToListAsync();
    }

    public async Task<bool> ExistsAsync(string gameId, string playerEmail, string productName)
    {
        var filter = Builders<WishlistItem>.Filter.And(
            Builders<WishlistItem>.Filter.Eq(x => x.GameId, gameId),
            Builders<WishlistItem>.Filter.Eq(x => x.PlayerEmail, playerEmail),
            Builders<WishlistItem>.Filter.Regex(x => x.ProductName, new MongoDB.Bson.BsonRegularExpression($"^{productName}$", "i"))
        );

        var count = await Collection.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task DeleteByGameAndPlayerAsync(string gameId, string playerEmail)
    {
        var filter = Builders<WishlistItem>.Filter.And(
            Builders<WishlistItem>.Filter.Eq(x => x.GameId, gameId),
            Builders<WishlistItem>.Filter.Eq(x => x.PlayerEmail, playerEmail)
        );

        await Collection.DeleteManyAsync(filter);
    }
}