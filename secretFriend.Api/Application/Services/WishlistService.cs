using secretFriend.Api.Application.DTOs;
using secretFriend.Api.Application.Interfaces;
using SecretFriend.Api.Application.Resources;
using secretFriend.Api.Domain.Entities;
using secretFriend.Api.Domain.Repositories;

namespace secretFriend.Api.Application.Services;

public class WishlistService(
    IWishlistRepository wishlistRepository,
    ISecretFriendGameRepository gameRepository) : IWishlistService
{
    public async Task<WishlistItemResponse> CreateWishlistItemAsync(string gameId, string playerEmail, CreateWishlistItemRequest request)
    {
        // Validar que el juego existe
        var game = await gameRepository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new ArgumentException(string.Format(Messages.GameNotFound, gameId));
        }

        // Validar que el jugador existe en el juego
        var playerExists = game.Players.Any(p => p.Email.Equals(playerEmail, StringComparison.OrdinalIgnoreCase));
        if (!playerExists)
        {
            throw new ArgumentException(string.Format(Messages.PlayerNotFound, playerEmail, gameId));
        }

        // Validar que no exista un producto duplicado
        var duplicateExists = await wishlistRepository.ExistsAsync(gameId, playerEmail, request.ProductName);
        if (duplicateExists)
        {
            throw new ArgumentException(string.Format(Messages.DuplicateWishlistItem, request.ProductName));
        }

        var wishlistItem = new WishlistItem
        {
            GameId = gameId,
            PlayerEmail = playerEmail,
            ProductName = request.ProductName,
            Description = request.Description,
            ApproximateValue = request.ApproximateValue,
            WebsiteLinks = request.WebsiteLinks ?? [],
            CreatedAt = DateTime.UtcNow
        };

        await wishlistRepository.InsertAsync(wishlistItem);

        return MapToResponse(wishlistItem);
    }

    public async Task<PlayerWishlistResponse> GetPlayerWishlistAsync(string gameId, string playerEmail)
    {
        // Validar que el juego existe
        var game = await gameRepository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new ArgumentException(string.Format(Messages.GameNotFound, gameId));
        }

        // Validar que el jugador existe en el juego
        var playerExists = game.Players.Any(p => p.Email.Equals(playerEmail, StringComparison.OrdinalIgnoreCase));
        if (!playerExists)
        {
            throw new ArgumentException(string.Format(Messages.PlayerNotFound, playerEmail, gameId));
        }

        var wishlistItems = await wishlistRepository.GetByGameAndPlayerAsync(gameId, playerEmail);
        var itemResponses = wishlistItems.Select(MapToResponse).ToList();

        return new PlayerWishlistResponse
        {
            GameId = gameId,
            PlayerEmail = playerEmail,
            Items = itemResponses,
            TotalItems = itemResponses.Count
        };
    }

    public async Task<WishlistItemResponse?> GetWishlistItemByIdAsync(string id)
    {
        var wishlistItem = await wishlistRepository.FindByIdAsync(id);
        return wishlistItem != null ? MapToResponse(wishlistItem) : null;
    }

    public async Task<WishlistItemResponse> UpdateWishlistItemAsync(string id, CreateWishlistItemRequest request)
    {
        var existingItem = await wishlistRepository.FindByIdAsync(id);
        if (existingItem == null)
        {
            throw new ArgumentException(Messages.WishlistItemNotFound);
        }

        // Validar duplicados solo si el nombre cambió
        if (!existingItem.ProductName.Equals(request.ProductName, StringComparison.OrdinalIgnoreCase))
        {
            var duplicateExists = await wishlistRepository.ExistsAsync(existingItem.GameId, existingItem.PlayerEmail, request.ProductName);
            if (duplicateExists)
            {
                throw new ArgumentException(string.Format(Messages.DuplicateWishlistItem, request.ProductName));
            }
        }

        existingItem.ProductName = request.ProductName;
        existingItem.Description = request.Description;
        existingItem.ApproximateValue = request.ApproximateValue;
        existingItem.WebsiteLinks = request.WebsiteLinks ?? [];
        existingItem.UpdatedAt = DateTime.UtcNow;

        await wishlistRepository.UpdateAsync(existingItem);

        return MapToResponse(existingItem);
    }

    public async Task<bool> DeleteWishlistItemAsync(string id)
    {
        var existingItem = await wishlistRepository.FindByIdAsync(id);
        if (existingItem == null)
        {
            return false;
        }

        await wishlistRepository.DeleteByIdAsync(id);
        return true;
    }

    private static WishlistItemResponse MapToResponse(WishlistItem item)
    {
        return new WishlistItemResponse
        {
            Id = item.Id,
            GameId = item.GameId,
            PlayerEmail = item.PlayerEmail,
            ProductName = item.ProductName,
            Description = item.Description,
            ApproximateValue = item.ApproximateValue,
            WebsiteLinks = item.WebsiteLinks,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}