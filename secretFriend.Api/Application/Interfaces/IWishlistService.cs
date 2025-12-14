using secretFriend.Api.Application.DTOs;

namespace secretFriend.Api.Application.Interfaces;

public interface IWishlistService
{
    Task<WishlistItemResponse> CreateWishlistItemAsync(string gameId, string playerEmail, CreateWishlistItemRequest request);
    Task<PlayerWishlistResponse> GetPlayerWishlistAsync(string gameId, string playerEmail);
    Task<WishlistItemResponse?> GetWishlistItemByIdAsync(string id);
    Task<WishlistItemResponse> UpdateWishlistItemAsync(string id, CreateWishlistItemRequest request);
    Task<bool> DeleteWishlistItemAsync(string id);
}