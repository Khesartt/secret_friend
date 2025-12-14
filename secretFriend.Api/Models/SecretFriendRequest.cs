using System.ComponentModel.DataAnnotations;

namespace secretFriend.Api.Models;

public class SecretFriendRequest
{
    [Required]
    [MinLength(2, ErrorMessage = "Se requieren al menos 2 jugadores para el amigo secreto")]
    public List<Player> Players { get; set; } = new();
}