using secretFriend.Api.Application.DTOs;
using secretFriend.Api.Domain.Entities;

namespace secretFriend.Api.Application.Interfaces;

public interface ISecretFriendService
{
    Task<SecretFriendResponse> GenerateAndSaveSecretFriendsAsync(SecretFriendRequest request);
    Task<IEnumerable<SecretFriendGame>> GetAllGamesAsync();
    Task<SecretFriendGame?> GetGameByIdAsync(string id);
}