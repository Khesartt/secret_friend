using System.ComponentModel.DataAnnotations;
using secretFriend.Api.Domain.Entities;

namespace secretFriend.Api.Application.DTOs;

public class SecretFriendRequest
{
    public string? GameName { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Se requieren al menos 2 jugadores para el amigo secreto")]
    public List<Player> Players { get; set; } = new();
}