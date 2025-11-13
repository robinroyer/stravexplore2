using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public interface ISegmentService
{
    Task<List<SegmentWithDifficultyDto>> ExploreByDifficultyAsync(
        double[] bounds,
        string activityType,
        string? difficulty = null);

    Task<SegmentAnalysisDto?> AnalyzeSegmentAsync(long segmentId);

    Task<List<SegmentComparisonDto>> CompareSegmentsAsync(List<long> segmentIds);

    Task<List<ExploreSegmentDto>> ExploreSegmentsAsync(
        double[] bounds,
        string activityType,
        int? minCat = null,
        int? maxCat = null);

    Task<LeaderboardDto?> GetSegmentLeaderboardAsync(long segmentId, int page = 1, int pageSize = 10);
}
