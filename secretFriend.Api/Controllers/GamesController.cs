using Microsoft.AspNetCore.Mvc;
using secretFriend.Api.Application.DTOs;
using secretFriend.Api.Application.Interfaces;
using secretFriend.Api.Domain.Entities;

namespace secretFriend.Api.Controllers.Games;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GamesController(ISecretFriendService secretFriendService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(SecretFriendResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretFriendResponse>> Post([FromBody] SecretFriendRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest($"Errores de validación: {string.Join("; ", errors)}");
            }

            if (request?.Players == null || !request.Players.Any())
            {
                return BadRequest("La lista de jugadores es requerida y no puede estar vacía");
            }

            var response = await secretFriendService.GenerateAndSaveSecretFriendsAsync(request);
            
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ha ocurrido un error inesperado. Por favor, intente nuevamente.");
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SecretFriendGame>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SecretFriendGame>>> Get()
    {
        var games = await secretFriendService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SecretFriendGame), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SecretFriendGame>> Get(string id)
    {
        var game = await secretFriendService.GetGameByIdAsync(id);
        
        if (game == null)
        {
            return NotFound($"No se encontró un juego con el ID: {id}");
        }
        
        return Ok(game);
    }
}