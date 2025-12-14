using MongoDB.Driver;
using secretFriend.Api.Domain.Entities;
using secretFriend.Api.Domain.Repositories;

namespace secretFriend.Api.Infrastructure.Persistence.MongoDb;

public class SecretFriendGameRepository(IMongoDatabase database) 
    : MongoRepositoryBase<SecretFriendGame>(database, MongoConstants.COLLECTION_SECRET_FRIENDS), ISecretFriendGameRepository
{
    public async Task<SecretFriendGame?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await FindByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<SecretFriendGame>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(cancellationToken);
    }

    public async Task SaveAsync(SecretFriendGame game, CancellationToken cancellationToken = default)
    {
        await InsertAsync(game, cancellationToken);
    }

    public new async Task UpdateAsync(SecretFriendGame game, CancellationToken cancellationToken = default)
    {
        await base.UpdateAsync(game, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await DeleteByIdAsync(id, cancellationToken);
    }
}