using secretFriend.Api.Domain.Entities;
using secretFriend.Api.Infrastructure.Persistence.MongoDb;

namespace secretFriend.Api.Domain.Repositories;

public interface IWishlistRepository : IMongoRepository<WishlistItem>
{
    Task<IEnumerable<WishlistItem>> GetByGameAndPlayerAsync(string gameId, string playerEmail);
    Task<bool> ExistsAsync(string gameId, string playerEmail, string productName);
    Task DeleteByGameAndPlayerAsync(string gameId, string playerEmail);
}