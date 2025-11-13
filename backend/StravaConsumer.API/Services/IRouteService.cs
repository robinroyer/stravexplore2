using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public interface IRouteService
{
    Task<List<RouteRecommendationDto>> GetPopularRoutesAsync(
        double lat,
        double lng,
        int radiusKm = 10);

    Task<RouteStatisticsDto?> GetRouteStatsAsync(long routeId);

    Task<List<RouteDto>> FilterRoutesByDifficultyAsync(
        double[] bounds,
        int minElevation,
        int maxElevation);

    Task<RouteDetailDto?> GetRouteDetailAsync(long routeId);
}
