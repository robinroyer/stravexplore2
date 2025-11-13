using System.Text.Json;
using StravaConsumer.API.Models.Dto;

namespace StravaConsumer.API.Services;

public class StravaApiService : IStravaApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StravaApiService> _logger;
    private readonly string _baseUrl;

    public StravaApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<StravaApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _baseUrl = _configuration["Strava:BaseUrl"] ?? "https://www.strava.com/api/v3";
    }

    public async Task<List<ExploreSegmentDto>> ExploreSegmentsAsync(
        double[] bounds,
        string activityType,
        int? minCat = null,
        int? maxCat = null)
    {
        try
        {
            // Note: Real API would require authentication
            // For this demo, we return mock data
            _logger.LogInformation(
                "Exploring segments: bounds={Bounds}, activityType={ActivityType}",
                string.Join(",", bounds),
                activityType);

            // Mock data for demonstration
            return GenerateMockExploreSegments(bounds, activityType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exploring segments");
            return new List<ExploreSegmentDto>();
        }
    }

    public async Task<SegmentDetailDto?> GetSegmentDetailAsync(long segmentId)
    {
        try
        {
            _logger.LogInformation("Getting segment detail: {SegmentId}", segmentId);

            // Mock data
            return GenerateMockSegmentDetail(segmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting segment detail for {SegmentId}", segmentId);
            return null;
        }
    }

    public async Task<LeaderboardDto?> GetSegmentLeaderboardAsync(
        long segmentId,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            _logger.LogInformation(
                "Getting leaderboard: segmentId={SegmentId}, page={Page}",
                segmentId,
                page);

            return GenerateMockLeaderboard(segmentId, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting leaderboard for {SegmentId}", segmentId);
            return null;
        }
    }

    public async Task<List<RouteDto>> GetAthleteRoutesAsync(
        long athleteId,
        int page = 1,
        int pageSize = 30)
    {
        try
        {
            _logger.LogInformation("Getting routes for athlete: {AthleteId}", athleteId);

            return GenerateMockRoutes(athleteId, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting routes for athlete {AthleteId}", athleteId);
            return new List<RouteDto>();
        }
    }

    public async Task<RouteDetailDto?> GetRouteDetailAsync(long routeId)
    {
        try
        {
            _logger.LogInformation("Getting route detail: {RouteId}", routeId);

            return GenerateMockRouteDetail(routeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting route detail for {RouteId}", routeId);
            return null;
        }
    }

    // Mock data generators
    private List<ExploreSegmentDto> GenerateMockExploreSegments(double[] bounds, string activityType)
    {
        var random = new Random();
        var segments = new List<ExploreSegmentDto>();

        for (int i = 1; i <= 20; i++)
        {
            var lat = bounds[0] + random.NextDouble() * (bounds[2] - bounds[0]);
            var lng = bounds[1] + random.NextDouble() * (bounds[3] - bounds[1]);

            segments.Add(new ExploreSegmentDto
            {
                Id = 1000000 + i,
                Name = $"Segment {i} - {activityType}",
                ClimbCategory = random.Next(0, 5).ToString(),
                ClimbCategoryDesc = GetClimbCategoryDesc(random.Next(0, 5)),
                AvgGrade = Math.Round(random.NextDouble() * 10, 1),
                Distance = Math.Round(random.NextDouble() * 5000, 0),
                ElevDifference = Math.Round(random.NextDouble() * 300, 0),
                StarCount = random.Next(0, 500),
                StartLatlng = new List<double> { lat, lng },
                EndLatlng = new List<double> { lat + 0.01, lng + 0.01 },
                Points = GenerateEncodedPolyline()
            });
        }

        return segments;
    }

    private SegmentDetailDto GenerateMockSegmentDetail(long segmentId)
    {
        var random = new Random((int)segmentId);

        return new SegmentDetailDto
        {
            Id = segmentId,
            Name = $"Segment Detail {segmentId}",
            ActivityType = random.Next(2) == 0 ? "Ride" : "Run",
            Distance = Math.Round(random.NextDouble() * 5000, 0),
            AverageGrade = Math.Round(random.NextDouble() * 10, 1),
            MaximumGrade = Math.Round(random.NextDouble() * 15 + 5, 1),
            ElevationHigh = Math.Round(random.NextDouble() * 1000 + 500, 0),
            ElevationLow = Math.Round(random.NextDouble() * 500, 0),
            ClimbCategory = random.Next(0, 5),
            City = "Paris",
            State = "Île-de-France",
            Country = "France",
            StarCount = random.Next(50, 1000),
            EffortCount = random.Next(1000, 50000),
            AthleteCount = random.Next(500, 10000),
            TotalElevationGain = Math.Round(random.NextDouble() * 500, 0),
            CreatedAt = DateTime.UtcNow.AddYears(-random.Next(1, 10)),
            UpdatedAt = DateTime.UtcNow,
            MapPolyline = GenerateEncodedPolyline()
        };
    }

    private LeaderboardDto GenerateMockLeaderboard(long segmentId, int pageSize)
    {
        var random = new Random((int)segmentId);
        var entries = new List<LeaderboardEntryDto>();

        for (int i = 1; i <= pageSize; i++)
        {
            entries.Add(new LeaderboardEntryDto
            {
                Rank = i,
                AthleteName = $"Athlete {i}",
                AthleteId = 100000 + i,
                ElapsedTime = random.Next(300, 3600),
                MovingTime = random.Next(300, 3600),
                StartDate = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
                ActivityId = 200000 + i,
                EffortId = 300000 + i
            });
        }

        return new LeaderboardDto
        {
            SegmentId = segmentId,
            EntryCount = entries.Count,
            Entries = entries
        };
    }

    private List<RouteDto> GenerateMockRoutes(long athleteId, int pageSize)
    {
        var random = new Random((int)athleteId);
        var routes = new List<RouteDto>();

        for (int i = 1; i <= Math.Min(pageSize, 10); i++)
        {
            routes.Add(new RouteDto
            {
                Id = 500000 + i,
                Name = $"Route {i}",
                Description = $"Mock route description {i}",
                AthleteId = athleteId,
                Distance = Math.Round(random.NextDouble() * 50000, 0),
                ElevationGain = Math.Round(random.NextDouble() * 1000, 0),
                Type = random.Next(2) == 0 ? "Ride" : "Run",
                SubType = 1,
                Private = false,
                Starred = random.Next(0, 100),
                Timestamp = DateTime.UtcNow.AddDays(-random.Next(1, 365))
            });
        }

        return routes;
    }

    private RouteDetailDto GenerateMockRouteDetail(long routeId)
    {
        var random = new Random((int)routeId);

        return new RouteDetailDto
        {
            Id = routeId,
            Name = $"Route Detail {routeId}",
            Description = $"Detailed description for route {routeId}",
            AthleteId = 123456,
            Distance = Math.Round(random.NextDouble() * 50000, 0),
            ElevationGain = Math.Round(random.NextDouble() * 1000, 0),
            Type = random.Next(2) == 0 ? "Ride" : "Run",
            SubType = 1,
            Private = false,
            Starred = random.Next(0, 100),
            Timestamp = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
            MapPolyline = GenerateEncodedPolyline(),
            EstimatedMovingTime = random.Next(1800, 10800),
            CreatedAt = DateTime.UtcNow.AddYears(-1),
            UpdatedAt = DateTime.UtcNow
        };
    }

    private string GetClimbCategoryDesc(int category)
    {
        return category switch
        {
            0 => "NC",
            1 => "4",
            2 => "3",
            3 => "2",
            4 => "1",
            _ => "HC"
        };
    }

    private string GenerateEncodedPolyline()
    {
        // Simple mock polyline - in real app this would be actual encoded polyline from Strava
        return "u{~vFvyqL?";
    }
}
