using secretFriend.Api.Domain.Entities;

namespace secretFriend.Api.Application.DTOs;

public class SecretFriendResponse
{
    public List<SecretFriendAssignment> Assignments { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}