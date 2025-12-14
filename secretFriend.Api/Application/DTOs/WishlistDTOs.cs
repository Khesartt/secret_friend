using System.ComponentModel.DataAnnotations;

namespace secretFriend.Api.Application.DTOs;

public class CreateWishlistItemRequest
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Approximate value must be greater than 0")]
    public decimal? ApproximateValue { get; set; }

    public List<string> WebsiteLinks { get; set; } = [];
}

public class WishlistItemResponse
{
    public string Id { get; set; } = string.Empty;
    public string GameId { get; set; } = string.Empty;
    public string PlayerEmail { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? ApproximateValue { get; set; }
    public List<string> WebsiteLinks { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PlayerWishlistResponse
{
    public string GameId { get; set; } = string.Empty;
    public string PlayerEmail { get; set; } = string.Empty;
    public List<WishlistItemResponse> Items { get; set; } = [];
    public int TotalItems { get; set; }
}