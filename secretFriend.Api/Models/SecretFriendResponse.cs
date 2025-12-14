namespace secretFriend.Api.Models;

public class SecretFriendAssignment
{
    public Player Giver { get; set; } = new();
    public Player Receiver { get; set; } = new();
}

public class SecretFriendResponse
{
    public List<SecretFriendAssignment> Assignments { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}