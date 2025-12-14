namespace secretFriend.Api.Domain.Entities;

public class SecretFriendAssignment
{
    public Player Giver { get; set; } = new();
    public Player Receiver { get; set; } = new();
}