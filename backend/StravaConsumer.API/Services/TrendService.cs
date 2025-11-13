using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public class TrendService : ITrendService
{
    private readonly IStravaApiService _stravaApiService;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TrendService> _logger;

    public TrendService(
        IStravaApiService stravaApiService,
        ICacheService cacheService,
        IConfiguration configuration,
        ILogger<TrendService> logger)
    {
        _stravaApiService = stravaApiService;
        _cacheService = cacheService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<TrendingSegmentDto>> GetTrendingSegmentsAsync(string region, int topN = 20)
    {
        var cacheKey = $"trending_segments_{region}_{topN}";
        var cached = _cacheService.Get<List<TrendingSegmentDto>>(cacheKey);

        if (cached != null)
        {
            _logger.LogInformation("Returning cached trending segments");
            return cached;
        }

        // Mock bounds based on region
        var bounds = GetBoundsForRegion(region);
        var segments = await _stravaApiService.ExploreSegmentsAsync(bounds, "Ride");

        var trendingSegments = segments.Select(s => new TrendingSegmentDto
        {
            SegmentId = s.Id,
            Name = s.Name,
            Distance = s.Distance,
            AverageGrade = s.AvgGrade,
            StarCount = s.StarCount,
            EffortCount = CalculateMockEffortCount(s.StarCount),
            AthleteCount = CalculateMockAthleteCount(s.StarCount),
            TrendScore = CalculateTrendScore(s.StarCount, CalculateMockEffortCount(s.StarCount)),
            City = "Mock City",
            Country = "Mock Country"
        })
        .OrderByDescending(t => t.TrendScore)
        .Take(topN)
        .ToList();

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:TrendsCacheDurationMinutes", 30));
        _cacheService.Set(cacheKey, trendingSegments, cacheDuration);

        return trendingSegments;
    }

    public async Task<SegmentTrendDataDto?> GetSegmentTrendAsync(long segmentId)
    {
        var cacheKey = $"segment_trend_{segmentId}";
        var cached = _cacheService.Get<SegmentTrendDataDto>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var segmentDetail = await _stravaApiService.GetSegmentDetailAsync(segmentId);
        if (segmentDetail == null)
        {
            return null;
        }

        // Generate mock trend data
        var trendData = new List<TrendDataPointDto>();
        var random = new Random((int)segmentId);

        for (int i = 30; i >= 0; i--)
        {
            trendData.Add(new TrendDataPointDto
            {
                Date = DateTime.UtcNow.AddDays(-i),
                EffortCount = random.Next(50, 500),
                AthleteCount = random.Next(20, 200)
            });
        }

        var segmentTrend = new SegmentTrendDataDto
        {
            SegmentId = segmentId,
            Name = segmentDetail.Name,
            TrendData = trendData,
            OverallTrend = CalculateOverallTrend(trendData)
        };

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:TrendsCacheDurationMinutes", 30));
        _cacheService.Set(cacheKey, segmentTrend, cacheDuration);

        return segmentTrend;
    }

    public async Task<List<EventSegmentDto>> FindEventSegmentsAsync(double[] bounds)
    {
        var cacheKey = $"event_segments_{string.Join("_", bounds)}";
        var cached = _cacheService.Get<List<EventSegmentDto>>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var segments = await _stravaApiService.ExploreSegmentsAsync(bounds, "Ride");

        var eventSegments = segments
            .Where(s => s.StarCount > 100) // Popular segments
            .Select(s => new EventSegmentDto
            {
                SegmentId = s.Id,
                Name = s.Name,
                Distance = s.Distance,
                StarCount = s.StarCount,
                EstimatedParticipants = s.StarCount * 5,
                PrCount = s.StarCount * 2,
                ActivityType = "Ride",
                StartLocation = s.StartLatlng,
                City = "Mock City",
                Country = "Mock Country"
            })
            .OrderByDescending(e => e.EstimatedParticipants)
            .Take(15)
            .ToList();

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:TrendsCacheDurationMinutes", 30));
        _cacheService.Set(cacheKey, eventSegments, cacheDuration);

        return eventSegments;
    }

    private double[] GetBoundsForRegion(string region)
    {
        // Mock bounds - in real app, would have a database of regions
        return region.ToLower() switch
        {
            "paris" => new double[] { 48.8, 2.3, 48.9, 2.4 },
            "lyon" => new double[] { 45.7, 4.8, 45.8, 4.9 },
            "marseille" => new double[] { 43.2, 5.3, 43.3, 5.4 },
            _ => new double[] { 48.8, 2.3, 48.9, 2.4 }
        };
    }

    private int CalculateMockEffortCount(int starCount)
    {
        return starCount * 100;
    }

    private int CalculateMockAthleteCount(int starCount)
    {
        return starCount * 20;
    }

    private double CalculateTrendScore(int starCount, int effortCount)
    {
        return (starCount * 0.3) + (effortCount * 0.001);
    }

    private double CalculateOverallTrend(List<TrendDataPointDto> trendData)
    {
        if (trendData.Count < 2)
            return 0;

        var firstWeek = trendData.Take(7).Average(t => t.EffortCount);
        var lastWeek = trendData.TakeLast(7).Average(t => t.EffortCount);

        if (firstWeek == 0)
            return 0;

        return ((lastWeek - firstWeek) / firstWeek) * 100;
    }
}
