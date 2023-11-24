using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Controllers;
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ICachingService _cachingService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ICachingService cachingService)
    {
        _logger = logger;
        _cachingService = cachingService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [AllowAnonymous]
    [HttpGet("get/{key}")]
    public async Task<string?> GetCache([FromRoute] string key, CancellationToken cancellationToken)
    {
        return await _cachingService.GetCachedDataAsync<string>(key, cancellationToken);

    }

    [AllowAnonymous]
    [HttpPost("set/{key}/{val}")]
    public async Task SetCache([FromRoute] string key, [FromRoute]string val, CancellationToken cancellationToken)
    {

        await _cachingService.SetCachedDataAsync(key, val, TimeSpan.FromSeconds(60), cancellationToken);

    }
}
