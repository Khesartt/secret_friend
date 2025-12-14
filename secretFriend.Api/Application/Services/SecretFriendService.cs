using secretFriend.Api.Application.DTOs;
using secretFriend.Api.Application.Interfaces;
using secretFriend.Api.Domain.Entities;
using secretFriend.Api.Domain.Repositories;
using SecretFriend.Api.Application.Resources;

namespace secretFriend.Api.Application.Services;

public class SecretFriendService(Random random, ISecretFriendGameRepository repository) : ISecretFriendService
{
    public async Task<SecretFriendResponse> GenerateAndSaveSecretFriendsAsync(SecretFriendRequest request)
    {
        var response = GenerateSecretFriends(request.Players);

        var game = new SecretFriendGame
        {
            Name = request.GameName ?? $"Amigo Secreto {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
            Players = request.Players,
            Assignments = response.Assignments,
            CreatedAt = DateTime.UtcNow
        };

        await repository.SaveAsync(game);

        return response;
    }

    public async Task<IEnumerable<SecretFriendGame>> GetAllGamesAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<SecretFriendGame?> GetGameByIdAsync(string id)
    {
        return await repository.GetByIdAsync(id);
    }

    private SecretFriendResponse GenerateSecretFriends(List<Player> players)
    {
        if (players.Count < 2)
        {
            throw new ArgumentException(Messages.MinimumPlayersRequired);
        }

        ValidatePlayers(players);

        var assignments = GenerateAssignments(players);

        var response = new SecretFriendResponse
        {
            Assignments = assignments,
            Message = string.Format(Messages.SecretFriendsGeneratedSuccessfully, players.Count),
            GeneratedAt = DateTime.UtcNow
        };

        return response;
    }

    private static void ValidatePlayers(List<Player> players)
    {
        var duplicateNames = players
            .GroupBy(p => p.Name.ToLowerInvariant().Trim())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateNames.Any())
        {
            throw new ArgumentException(string.Format(Messages.DuplicateNamesFound, string.Join(", ", duplicateNames)));
        }

        var duplicateEmails = players
            .GroupBy(p => p.Email.ToLowerInvariant().Trim())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateEmails.Any())
        {
            throw new ArgumentException(string.Format(Messages.DuplicateEmailsFound, string.Join(", ", duplicateEmails)));
        }

        var invalidPlayers = players
            .Where(p => string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Email))
            .ToList();

        if (invalidPlayers.Any())
        {
            throw new ArgumentException(Messages.InvalidPlayersData);
        }
    }

    private List<SecretFriendAssignment> GenerateAssignments(List<Player> players)
    {
        const int maxAttempts = 1000;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var shuffledReceivers = new List<Player>(players);
            ShuffleList(shuffledReceivers);

            bool isValidAssignment = true;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Email.Equals(shuffledReceivers[i].Email, StringComparison.OrdinalIgnoreCase))
                {
                    isValidAssignment = false;
                    break;
                }
            }

            if (isValidAssignment)
            {
                var assignments = new List<SecretFriendAssignment>();
                for (int i = 0; i < players.Count; i++)
                {
                    assignments.Add(new SecretFriendAssignment
                    {
                        Giver = new Player { Name = players[i].Name, Email = players[i].Email },
                        Receiver = new Player { Name = shuffledReceivers[i].Name, Email = shuffledReceivers[i].Email }
                    });
                }

                return assignments;
            }
        }

        throw new InvalidOperationException(string.Format(Messages.AssignmentGenerationFailed, maxAttempts));
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}