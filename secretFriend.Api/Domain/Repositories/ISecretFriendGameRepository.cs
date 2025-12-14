using secretFriend.Api.Domain.Entities;

namespace secretFriend.Api.Domain.Repositories;

public interface ISecretFriendGameRepository
{
    Task<SecretFriendGame?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SecretFriendGame>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(SecretFriendGame game, CancellationToken cancellationToken = default);
    Task UpdateAsync(SecretFriendGame game, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}