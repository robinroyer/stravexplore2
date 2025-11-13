using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public class RouteService : IRouteService
{
    private readonly IStravaApiService _stravaApiService;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RouteService> _logger;

    public RouteService(
        IStravaApiService stravaApiService,
        ICacheService cacheService,
        IConfiguration configuration,
        ILogger<RouteService> logger)
    {
        _stravaApiService = stravaApiService;
        _cacheService = cacheService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<RouteRecommendationDto>> GetPopularRoutesAsync(
        double lat,
        double lng,
        int radiusKm = 10)
    {
        var cacheKey = $"routes_popular_{lat}_{lng}_{radiusKm}";
        var cached = _cacheService.Get<List<RouteRecommendationDto>>(cacheKey);

        if (cached != null)
        {
            _logger.LogInformation("Returning cached popular routes");
            return cached;
        }

        // Mock athlete IDs for demo - in real app, would search by location
        var athleteIds = new List<long> { 12345, 67890, 11111 };
        var allRoutes = new List<RouteRecommendationDto>();

        foreach (var athleteId in athleteIds)
        {
            var routes = await _stravaApiService.GetAthleteRoutesAsync(athleteId);
            var recommendations = routes.Select(r => new RouteRecommendationDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                AthleteId = r.AthleteId,
                Distance = r.Distance,
                ElevationGain = r.ElevationGain,
                Type = r.Type,
                SubType = r.SubType,
                Private = r.Private,
                Starred = r.Starred,
                Timestamp = r.Timestamp,
                PopularityScore = CalculatePopularityScore(r.Starred, r.Distance),
                UserCount = r.Starred * 10,
                DifficultyLevel = CalculateRouteDifficulty(r.Distance, r.ElevationGain)
            }).ToList();

            allRoutes.AddRange(recommendations);
        }

        var popularRoutes = allRoutes
            .OrderByDescending(r => r.PopularityScore)
            .Take(20)
            .ToList();

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:RouteCacheDurationMinutes", 120));
        _cacheService.Set(cacheKey, popularRoutes, cacheDuration);

        return popularRoutes;
    }

    public async Task<RouteStatisticsDto?> GetRouteStatsAsync(long routeId)
    {
        var cacheKey = $"route_stats_{routeId}";
        var cached = _cacheService.Get<RouteStatisticsDto>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var routeDetail = await _stravaApiService.GetRouteDetailAsync(routeId);
        if (routeDetail == null)
        {
            return null;
        }

        var stats = new RouteStatisticsDto
        {
            RouteId = routeDetail.Id,
            Name = routeDetail.Name,
            Distance = routeDetail.Distance,
            ElevationGain = routeDetail.ElevationGain,
            AverageMovingTime = routeDetail.EstimatedMovingTime,
            TotalUsers = routeDetail.Starred * 10,
            Type = routeDetail.Type,
            Segments = new List<SegmentDto>()
        };

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:RouteCacheDurationMinutes", 120));
        _cacheService.Set(cacheKey, stats, cacheDuration);

        return stats;
    }

    public async Task<List<RouteDto>> FilterRoutesByDifficultyAsync(
        double[] bounds,
        int minElevation,
        int maxElevation)
    {
        var cacheKey = $"routes_filter_{string.Join("_", bounds)}_{minElevation}_{maxElevation}";
        var cached = _cacheService.Get<List<RouteDto>>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        // Mock athlete IDs for demo
        var athleteIds = new List<long> { 12345, 67890 };
        var allRoutes = new List<RouteDto>();

        foreach (var athleteId in athleteIds)
        {
            var routes = await _stravaApiService.GetAthleteRoutesAsync(athleteId);
            allRoutes.AddRange(routes);
        }

        var filteredRoutes = allRoutes
            .Where(r => r.ElevationGain >= minElevation && r.ElevationGain <= maxElevation)
            .ToList();

        var cacheDuration = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Caching:RouteCacheDurationMinutes", 120));
        _cacheService.Set(cacheKey, filteredRoutes, cacheDuration);

        return filteredRoutes;
    }

    public async Task<RouteDetailDto?> GetRouteDetailAsync(long routeId)
    {
        var cacheKey = $"route_detail_{routeId}";
        var cached = _cacheService.Get<RouteDetailDto>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var routeDetail = await _stravaApiService.GetRouteDetailAsync(routeId);

        if (routeDetail != null)
        {
            var cacheDuration = TimeSpan.FromMinutes(
                _configuration.GetValue<int>("Caching:RouteCacheDurationMinutes", 120));
            _cacheService.Set(cacheKey, routeDetail, cacheDuration);
        }

        return routeDetail;
    }

    private double CalculatePopularityScore(int starred, double distance)
    {
        // Simple popularity score based on stars and distance normalization
        var starScore = starred * 10;
        var distanceScore = distance / 1000;
        return starScore + distanceScore;
    }

    private string CalculateRouteDifficulty(double distance, double elevationGain)
    {
        var score = (distance / 1000) + (elevationGain / 10);

        return score switch
        {
            < 20 => "Easy",
            < 40 => "Moderate",
            < 60 => "Hard",
            _ => "Very Hard"
        };
    }
}
