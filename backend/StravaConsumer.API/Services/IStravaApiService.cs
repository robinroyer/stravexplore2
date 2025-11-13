using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public interface IStravaApiService
{
    Task<List<ExploreSegmentDto>> ExploreSegmentsAsync(
        double[] bounds,
        string activityType,
        int? minCat = null,
        int? maxCat = null);

    Task<SegmentDetailDto?> GetSegmentDetailAsync(long segmentId);

    Task<LeaderboardDto?> GetSegmentLeaderboardAsync(
        long segmentId,
        int page = 1,
        int pageSize = 10);

    Task<List<RouteDto>> GetAthleteRoutesAsync(
        long athleteId,
        int page = 1,
        int pageSize = 30);

    Task<RouteDetailDto?> GetRouteDetailAsync(long routeId);
}
