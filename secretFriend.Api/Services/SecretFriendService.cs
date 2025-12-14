using secretFriend.Api.Models;

namespace secretFriend.Api.Services;

public class SecretFriendService(Random random) : ISecretFriendService
{
    public SecretFriendResponse GenerateSecretFriends(List<Player> players)
    {
        if (players.Count < 2)
        {
            throw new ArgumentException("Se requieren al menos 2 jugadores para el amigo secreto");
        }

        ValidatePlayers(players);

        var assignments = GenerateAssignments(players);

        var response = new SecretFriendResponse
        {
            Assignments = assignments,
            Message = $"¡Amigos secretos generados exitosamente para {players.Count} jugadores!",
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
            throw new ArgumentException($"Nombres duplicados encontrados: {string.Join(", ", duplicateNames)}");
        }

        var duplicateEmails = players
            .GroupBy(p => p.Email.ToLowerInvariant().Trim())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateEmails.Any())
        {
            throw new ArgumentException($"Emails duplicados encontrados: {string.Join(", ", duplicateEmails)}");
        }

        var invalidPlayers = players
            .Where(p => string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Email))
            .ToList();

        if (invalidPlayers.Any())
        {
            throw new ArgumentException("Todos los jugadores deben tener nombre y email válidos");
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

        throw new InvalidOperationException($"No se pudo generar una asignación válida después de {maxAttempts} intentos");
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