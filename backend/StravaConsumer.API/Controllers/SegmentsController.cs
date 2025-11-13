using Microsoft.AspNetCore.Mvc;
using StravaConsumer.API.Services;

namespace StravaConsumer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SegmentsController : ControllerBase
{
    private readonly ISegmentService _segmentService;
    private readonly ILogger<SegmentsController> _logger;

    public SegmentsController(
        ISegmentService segmentService,
        ILogger<SegmentsController> logger)
    {
        _segmentService = segmentService;
        _logger = logger;
    }

    [HttpGet("explore")]
    public async Task<IActionResult> Explore(
        [FromQuery] double minLat,
        [FromQuery] double minLng,
        [FromQuery] double maxLat,
        [FromQuery] double maxLng,
        [FromQuery] string activityType = "Ride",
        [FromQuery] int? minCat = null,
        [FromQuery] int? maxCat = null)
    {
        try
        {
            var bounds = new double[] { minLat, minLng, maxLat, maxLng };
            var segments = await _segmentService.ExploreSegmentsAsync(bounds, activityType, minCat, maxCat);
            return Ok(segments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exploring segments");
            return StatusCode(500, new { error = "Failed to explore segments" });
        }
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(long id)
    {
        try
        {
            var analysis = await _segmentService.AnalyzeSegmentAsync(id);
            if (analysis == null)
            {
                return NotFound(new { error = "Segment not found" });
            }
            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting segment details for {SegmentId}", id);
            return StatusCode(500, new { error = "Failed to get segment details" });
        }
    }

    [HttpGet("{id}/leaderboard")]
    public async Task<IActionResult> GetLeaderboard(
        long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var leaderboard = await _segmentService.GetSegmentLeaderboardAsync(id, page, pageSize);
            if (leaderboard == null)
            {
                return NotFound(new { error = "Leaderboard not found" });
            }
            return Ok(leaderboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting leaderboard for segment {SegmentId}", id);
            return StatusCode(500, new { error = "Failed to get leaderboard" });
        }
    }

    [HttpGet("difficulty-analyzer")]
    public async Task<IActionResult> AnalyzeByDifficulty(
        [FromQuery] double minLat,
        [FromQuery] double minLng,
        [FromQuery] double maxLat,
        [FromQuery] double maxLng,
        [FromQuery] string activityType = "Ride",
        [FromQuery] string? difficulty = null)
    {
        try
        {
            var bounds = new double[] { minLat, minLng, maxLat, maxLng };
            var segments = await _segmentService.ExploreByDifficultyAsync(bounds, activityType, difficulty);
            return Ok(segments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing segments by difficulty");
            return StatusCode(500, new { error = "Failed to analyze segments" });
        }
    }

    [HttpPost("compare")]
    public async Task<IActionResult> CompareSegments([FromBody] List<long> segmentIds)
    {
        try
        {
            if (segmentIds == null || segmentIds.Count == 0)
            {
                return BadRequest(new { error = "Segment IDs required" });
            }

            var comparison = await _segmentService.CompareSegmentsAsync(segmentIds);
            return Ok(comparison);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error comparing segments");
            return StatusCode(500, new { error = "Failed to compare segments" });
        }
    }
}
