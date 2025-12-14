using Microsoft.AspNetCore.Mvc;

namespace secretFriend.Api.Controllers.Info;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InfoController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult<object> Get()
    {
        return Ok(new
        {
            name = "Secret Friend API",
            description = "API para generar asignaciones de amigo secreto de forma aleatoria",
            version = "1.0.0",
            endpoints = new[]
            {
                new { method = "POST", path = "/api/games", description = "Genera asignaciones de amigo secreto" },
                new { method = "GET", path = "/api/games", description = "Obtiene todos los juegos" },
                new { method = "GET", path = "/api/games/{id}", description = "Obtiene un juego por ID" },
                new { method = "GET", path = "/api/health", description = "Verifica la salud del servicio" },
                new { method = "GET", path = "/api/info", description = "Información del API" }
            },
            sampleRequest = new
            {
                gameName = "Navidad 2025",
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