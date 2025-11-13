using Microsoft.AspNetCore.Mvc;
using StravaConsumer.API.Services;

namespace StravaConsumer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrendsController : ControllerBase
{
    private readonly ITrendService _trendService;
    private readonly ILogger<TrendsController> _logger;

    public TrendsController(
        ITrendService trendService,
        ILogger<TrendsController> logger)
    {
        _trendService = trendService;
        _logger = logger;
    }

    [HttpGet("segments")]
    public async Task<IActionResult> GetTrendingSegments(
        [FromQuery] string region = "paris",
        [FromQuery] int topN = 20)
    {
        try
        {
            var segments = await _trendService.GetTrendingSegmentsAsync(region, topN);
            return Ok(segments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trending segments");
            return StatusCode(500, new { error = "Failed to get trending segments" });
        }
    }

    [HttpGet("segment/{id}/history")]
    public async Task<IActionResult> GetSegmentTrend(long id)
    {
        try
        {
            var trend = await _trendService.GetSegmentTrendAsync(id);
            if (trend == null)
            {
                return NotFound(new { error = "Segment trend not found" });
            }
            return Ok(trend);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting segment trend for {SegmentId}", id);
            return StatusCode(500, new { error = "Failed to get segment trend" });
        }
    }

    [HttpGet("events")]
    public async Task<IActionResult> FindEvents(
        [FromQuery] double minLat,
        [FromQuery] double minLng,
        [FromQuery] double maxLat,
        [FromQuery] double maxLng)
    {
        try
        {
            var bounds = new double[] { minLat, minLng, maxLat, maxLng };
            var events = await _trendService.FindEventSegmentsAsync(bounds);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding event segments");
            return StatusCode(500, new { error = "Failed to find events" });
        }
    }
}
