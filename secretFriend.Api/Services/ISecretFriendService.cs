using secretFriend.Api.Models;

namespace secretFriend.Api.Services;

public interface ISecretFriendService
{
    SecretFriendResponse GenerateSecretFriends(List<Player> players);
}