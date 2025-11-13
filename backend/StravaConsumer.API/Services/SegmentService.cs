using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public class SegmentService : ISegmentService
{
    private readonly IStravaApiService _stravaApiService;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SegmentService> _logger;

    public SegmentService(
        IStravaApiService stravaApiService,
        ICacheService cacheService,
        IConfiguration configuration,
        ILogger<SegmentService> logger)
    {
        _stravaApiService = stravaApiService;
        _cacheService = cacheService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<SegmentWithDifficultyDto>> ExploreByDifficultyAsync(
        double[] bounds,
        string activityType,
        string? difficulty = null)
    {
        var cacheKey = $"segments_difficulty_{string.Join("_", bounds)}_{activityType}_{difficulty}";
        var cached = _cacheService.Get<List<SegmentWithDifficultyDto>>(cacheKey);

        if (cached != null)
        {
            _logger.LogInformation("Returning cached segments by difficulty");
            return cached;
        }

        var segments = await _stravaApiService.ExploreSegmentsAsync(bounds, activityType);
        var segmentsWithDifficulty = segments.Select(s => new SegmentWithDifficultyDto
        {
            Id = s.Id,
            Name = s.Name,
            ActivityType = activityType,
            Distance = s.Distance,
            AverageGrade = s.AvgGrade,
            MaximumGrade = s.AvgGrade * 1.5,
            StarCount = s.StarCount,
            DifficultyLevel = CalculateDifficultyLevel(s.AvgGrade, s.Distance, s.ElevDifference),
            DifficultyScore = CalculateDifficultyScore(s.AvgGrade, s.Distance, s.ElevDifference)
        }).ToList();

        if (!string.IsNullOrEmpty(difficulty))
        {
            segmentsWithDifficulty = segmentsWithDifficulty
                .Where(s => s.DifficultyLevel.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:SegmentCacheDurationMinutes", 60));
        _cacheService.Set(cacheKey, segmentsWithDifficulty, cacheDuration);

        return segmentsWithDifficulty;
    }

    public async Task<SegmentAnalysisDto?> AnalyzeSegmentAsync(long segmentId)
    {
        var cacheKey = $"segment_analysis_{segmentId}";
        var cached = _cacheService.Get<SegmentAnalysisDto>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var segmentDetail = await _stravaApiService.GetSegmentDetailAsync(segmentId);
        if (segmentDetail == null)
        {
            return null;
        }

        var leaderboard = await _stravaApiService.GetSegmentLeaderboardAsync(segmentId);

        var analysis = new SegmentAnalysisDto
        {
            SegmentId = segmentDetail.Id,
            Name = segmentDetail.Name,
            Distance = segmentDetail.Distance,
            AverageGrade = segmentDetail.AverageGrade,
            MaximumGrade = segmentDetail.MaximumGrade,
            TotalElevationGain = segmentDetail.TotalElevationGain,
            DifficultyLevel = CalculateDifficultyLevel(
                segmentDetail.AverageGrade,
                segmentDetail.Distance,
                segmentDetail.TotalElevationGain),
            EstimatedTimeMinutes = EstimateTime(segmentDetail.Distance, segmentDetail.AverageGrade),
            Leaderboard = leaderboard
        };

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:SegmentCacheDurationMinutes", 60));
        _cacheService.Set(cacheKey, analysis, cacheDuration);

        return analysis;
    }

    public async Task<List<SegmentComparisonDto>> CompareSegmentsAsync(List<long> segmentIds)
    {
        var comparisons = new List<SegmentComparisonDto>();

        foreach (var segmentId in segmentIds)
        {
            var detail = await _stravaApiService.GetSegmentDetailAsync(segmentId);
            if (detail != null)
            {
                comparisons.Add(new SegmentComparisonDto
                {
                    SegmentId = detail.Id,
                    Name = detail.Name,
                    Distance = detail.Distance,
                    AverageGrade = detail.AverageGrade,
                    ElevationGain = detail.TotalElevationGain,
                    AthleteCount = detail.AthleteCount,
                    DifficultyLevel = CalculateDifficultyLevel(
                        detail.AverageGrade,
                        detail.Distance,
                        detail.TotalElevationGain)
                });
            }
        }

        return comparisons;
    }

    public async Task<List<ExploreSegmentDto>> ExploreSegmentsAsync(
        double[] bounds,
        string activityType,
        int? minCat = null,
        int? maxCat = null)
    {
        var cacheKey = $"segments_explore_{string.Join("_", bounds)}_{activityType}_{minCat}_{maxCat}";
        var cached = _cacheService.Get<List<ExploreSegmentDto>>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var segments = await _stravaApiService.ExploreSegmentsAsync(bounds, activityType, minCat, maxCat);

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:SegmentCacheDurationMinutes", 60));
        _cacheService.Set(cacheKey, segments, cacheDuration);

        return segments;
    }

    public async Task<LeaderboardDto?> GetSegmentLeaderboardAsync(
        long segmentId,
        int page = 1,
        int pageSize = 10)
    {
        var cacheKey = $"segment_leaderboard_{segmentId}_{page}_{pageSize}";
        var cached = _cacheService.Get<LeaderboardDto>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var leaderboard = await _stravaApiService.GetSegmentLeaderboardAsync(segmentId, page, pageSize);

        if (leaderboard != null)
        {
            var cacheDuration = TimeSpan.FromMinutes(30);
            _cacheService.Set(cacheKey, leaderboard, cacheDuration);
        }

        return leaderboard;
    }

    private string CalculateDifficultyLevel(double avgGrade, double distance, double elevationGain)
    {
        var score = CalculateDifficultyScore(avgGrade, distance, elevationGain);

        return score switch
        {
            < 20 => "Easy",
            < 40 => "Moderate",
            < 60 => "Hard",
            < 80 => "Very Hard",
            _ => "Extreme"
        };
    }

    private double CalculateDifficultyScore(double avgGrade, double distance, double elevationGain)
    {
        // Simple difficulty calculation
        var gradeScore = avgGrade * 5;
        var distanceScore = (distance / 1000) * 2;
        var elevationScore = (elevationGain / 100) * 3;

        return Math.Min(100, gradeScore + distanceScore + elevationScore);
    }

    private double EstimateTime(double distance, double avgGrade)
    {
        // Estimate time in minutes based on distance and grade
        var baseSpeed = 15.0; // km/h
        var gradeAdjustment = 1 + (avgGrade / 10);
        var timeHours = (distance / 1000) / (baseSpeed / gradeAdjustment);
        return timeHours * 60;
    }
}
