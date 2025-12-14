using Microsoft.AspNetCore.Mvc;
using secretFriend.Api.Models;
using secretFriend.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace secretFriend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SecretFriendController(ISecretFriendService secretFriendService, ILogger<SecretFriendController> logger) : ControllerBase
{
    [HttpPost("generate")]
    [ProducesResponseType(typeof(SecretFriendResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretFriendResponse>> GenerateSecretFriends([FromBody] SecretFriendRequest request)
    {
        try
        {
            logger.LogInformation("Recibida solicitud para generar amigos secretos con {Count} jugadores", 
                request?.Players?.Count ?? 0);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                logger.LogWarning("Modelo inválido: {Errors}", string.Join("; ", errors));
                return BadRequest($"Errores de validación: {string.Join("; ", errors)}");
            }

            if (request?.Players == null || !request.Players.Any())
            {
                return BadRequest("La lista de jugadores es requerida y no puede estar vacía");
            }

            var response = secretFriendService.GenerateSecretFriends(request.Players);
            
            logger.LogInformation("Amigos secretos generados exitosamente para {Count} jugadores", 
                request.Players.Count);
            
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning("Error de validación: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError("Error de operación: {Message}", ex.Message);
            return StatusCode(500, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al generar amigos secretos");
            return StatusCode(500, "Ha ocurrido un error inesperado. Por favor, intente nuevamente.");
        }
    }

    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new 
        { 
            status = "healthy",
            service = "SecretFriend API",
            timestamp = DateTime.UtcNow,
            message = "El servicio de amigo secreto está funcionando correctamente"
        });
    }

    [HttpGet("info")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult<object> GetApiInfo()
    {
        return Ok(new
        {
            name = "Secret Friend API",
            description = "API para generar asignaciones de amigo secreto de forma aleatoria",
            version = "1.0.0",
            endpoints = new[]
            {
                new { method = "POST", path = "/api/secretfriend/generate", description = "Genera asignaciones de amigo secreto" },
                new { method = "GET", path = "/api/secretfriend/health", description = "Verifica la salud del servicio" },
                new { method = "GET", path = "/api/secretfriend/info", description = "Información del API" }
            },
            sampleRequest = new
            {
                players = new[]
                {
                    new { name = "Juan Pérez", email = "juan@ejemplo.com" },
                    new { name = "María García", email = "maria@ejemplo.com" },
                    new { name = "Carlos López", email = "carlos@ejemplo.com" }
                }
            }
        });
    }
}