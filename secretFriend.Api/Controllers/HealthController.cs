using Microsoft.AspNetCore.Mvc;

namespace secretFriend.Api.Controllers.Health;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult<object> Get()
    {
        return Ok(new 
        { 
            status = "healthy",
            service = "SecretFriend API",
            timestamp = DateTime.UtcNow,
            message = "El servicio de amigo secreto está funcionando correctamente"
        });
    }
}