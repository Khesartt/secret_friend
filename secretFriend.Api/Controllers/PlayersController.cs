using Microsoft.AspNetCore.Mvc;
using secretFriend.Api.Application.DTOs;
using secretFriend.Api.Application.Interfaces;
using SecretFriend.Api.Application.Resources;

namespace secretFriend.Api.Controllers.Players;

[ApiController]
[Route("api/games/{gameId}/players/{playerEmail}")]
[Produces("application/json")]
public class PlayersController(IWishlistService wishlistService) : ControllerBase
{
    [HttpPost("wishlist")]
    [ProducesResponseType(typeof(WishlistItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WishlistItemResponse>> CreateWishlistItem(
        string gameId,
        string playerEmail,
        [FromBody] CreateWishlistItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(string.Format(Messages.ValidationErrors, string.Join("; ", errors)));
            }

            var response = await wishlistService.CreateWishlistItemAsync(gameId, playerEmail, request);
            
            return CreatedAtAction(
                nameof(GetWishlistItem),
                new { gameId, playerEmail, id = response.Id },
                response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Messages.UnexpectedError);
        }
    }

    [HttpGet("wishlist")]
    [ProducesResponseType(typeof(PlayerWishlistResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlayerWishlistResponse>> GetPlayerWishlist(
        string gameId,
        string playerEmail)
    {
        try
        {
            var response = await wishlistService.GetPlayerWishlistAsync(gameId, playerEmail);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Messages.UnexpectedError);
        }
    }

    [HttpGet("wishlist/{id}")]
    [ProducesResponseType(typeof(WishlistItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WishlistItemResponse>> GetWishlistItem(
        string gameId,
        string playerEmail,
        string id)
    {
        try
        {
            var response = await wishlistService.GetWishlistItemByIdAsync(id);
            
            if (response == null)
            {
                return NotFound(Messages.WishlistItemNotFound);
            }

            // Validar que el item pertenece al juego y jugador correcto
            if (response.GameId != gameId || !response.PlayerEmail.Equals(playerEmail, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(Messages.WishlistItemNotFound);
            }

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, Messages.UnexpectedError);
        }
    }

    [HttpPut("wishlist/{id}")]
    [ProducesResponseType(typeof(WishlistItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WishlistItemResponse>> UpdateWishlistItem(
        string gameId,
        string playerEmail,
        string id,
        [FromBody] CreateWishlistItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(string.Format(Messages.ValidationErrors, string.Join("; ", errors)));
            }

            // Verificar que el item existe y pertenece al juego y jugador
            var existingItem = await wishlistService.GetWishlistItemByIdAsync(id);
            if (existingItem == null || 
                existingItem.GameId != gameId || 
                !existingItem.PlayerEmail.Equals(playerEmail, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(Messages.WishlistItemNotFound);
            }

            var response = await wishlistService.UpdateWishlistItemAsync(id, request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Messages.UnexpectedError);
        }
    }

    [HttpDelete("wishlist/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWishlistItem(
        string gameId,
        string playerEmail,
        string id)
    {
        try
        {
            // Verificar que el item existe y pertenece al juego y jugador
            var existingItem = await wishlistService.GetWishlistItemByIdAsync(id);
            if (existingItem == null || 
                existingItem.GameId != gameId || 
                !existingItem.PlayerEmail.Equals(playerEmail, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(Messages.WishlistItemNotFound);
            }

            var deleted = await wishlistService.DeleteWishlistItemAsync(id);
            return deleted ? NoContent() : NotFound(Messages.WishlistItemNotFound);
        }
        catch (Exception)
        {
            return StatusCode(500, Messages.UnexpectedError);
        }
    }
}