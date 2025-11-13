namespace StravaConsumer.API.Models.Dto;

public class RouteDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long AthleteId { get; set; }
    public double Distance { get; set; }
    public double ElevationGain { get; set; }
    public string Type { get; set; } = string.Empty;
    public int SubType { get; set; }
    public bool Private { get; set; }
    public int Starred { get; set; }
    public DateTime Timestamp { get; set; }
    public List<double[]> Coordinates { get; set; } = new();
}

public class RouteDetailDto : RouteDto
{
    public string? MapPolyline { get; set; }
    public List<long> SegmentIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public double EstimatedMovingTime { get; set; }
}

public class RouteRecommendationDto : RouteDto
{
    public double PopularityScore { get; set; }
    public int UserCount { get; set; }
    public string DifficultyLevel { get; set; } = string.Empty;
}

public class RouteStatisticsDto
{
    public long RouteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double ElevationGain { get; set; }
    public double AverageMovingTime { get; set; }
    public int TotalUsers { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<SegmentDto> Segments { get; set; } = new();
}
