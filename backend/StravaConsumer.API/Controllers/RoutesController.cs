using Microsoft.AspNetCore.Mvc;
using StravaConsumer.API.Services;

namespace StravaConsumer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly IRouteService _routeService;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(
        IRouteService routeService,
        ILogger<RoutesController> logger)
    {
        _routeService = routeService;
        _logger = logger;
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular(
        [FromQuery] double lat,
        [FromQuery] double lng,
        [FromQuery] int radiusKm = 10)
    {
        try
        {
            var routes = await _routeService.GetPopularRoutesAsync(lat, lng, radiusKm);
            return Ok(routes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting popular routes");
            return StatusCode(500, new { error = "Failed to get popular routes" });
        }
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(long id)
    {
        try
        {
            var route = await _routeService.GetRouteDetailAsync(id);
            if (route == null)
            {
                return NotFound(new { error = "Route not found" });
            }
            return Ok(route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting route details for {RouteId}", id);
            return StatusCode(500, new { error = "Failed to get route details" });
        }
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetStats(long id)
    {
        try
        {
            var stats = await _routeService.GetRouteStatsAsync(id);
            if (stats == null)
            {
                return NotFound(new { error = "Route not found" });
            }
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting route stats for {RouteId}", id);
            return StatusCode(500, new { error = "Failed to get route stats" });
        }
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterByDifficulty(
        [FromQuery] double minLat,
        [FromQuery] double minLng,
        [FromQuery] double maxLat,
        [FromQuery] double maxLng,
        [FromQuery] int minElevation = 0,
        [FromQuery] int maxElevation = 5000)
    {
        try
        {
            var bounds = new double[] { minLat, minLng, maxLat, maxLng };
            var routes = await _routeService.FilterRoutesByDifficultyAsync(bounds, minElevation, maxElevation);
            return Ok(routes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering routes");
            return StatusCode(500, new { error = "Failed to filter routes" });
        }
    }
}
