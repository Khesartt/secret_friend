namespace secretFriend.Api.Domain.Entities;

public class SecretFriendGame
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public List<Player> Players { get; set; } = new();
    public List<SecretFriendAssignment> Assignments { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}