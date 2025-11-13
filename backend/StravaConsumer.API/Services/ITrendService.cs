using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public interface ITrendService
{
    Task<List<TrendingSegmentDto>> GetTrendingSegmentsAsync(string region, int topN = 20);
    Task<SegmentTrendDataDto?> GetSegmentTrendAsync(long segmentId);
    Task<List<EventSegmentDto>> FindEventSegmentsAsync(double[] bounds);
}
