using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/")]
public class PublicController(ILogger<PublicController> logger, AppDbContext context) : ControllerBase
{
    private readonly ILogger<PublicController> _logger = logger;
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Check if the API is running and if it can connect to the database.
    /// </summary>
    /// <returns>An information indicating if the service is up and running.</returns>
    /// <response code="200">API is online and can establish a connection to the database.</response>
    /// <response code="503">API is online but cannot connect to the database. Check if database server is online and the ConnectionString in appsettings.json.</response>

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> GetStatus()
    {
        var result = await _context.Database.CanConnectAsync();

        return result ? StatusCode(StatusCodes.Status200OK, "API and DB are up and running!") : StatusCode(StatusCodes.Status503ServiceUnavailable, "Can't connect to database!");
    }
}