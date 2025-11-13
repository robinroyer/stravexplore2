namespace StravaConsumer.API.Models.Dto;

public class SegmentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double AverageGrade { get; set; }
    public double MaximumGrade { get; set; }
    public double ElevationHigh { get; set; }
    public double ElevationLow { get; set; }
    public int ClimbCategory { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<double[]> Coordinates { get; set; } = new();
    public int StarCount { get; set; }
    public int EffortCount { get; set; }
    public int AthleteCount { get; set; }
}

public class SegmentDetailDto : SegmentDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public double TotalElevationGain { get; set; }
    public string? MapPolyline { get; set; }
}

public class SegmentWithDifficultyDto : SegmentDto
{
    public string DifficultyLevel { get; set; } = string.Empty;
    public double DifficultyScore { get; set; }
}

public class SegmentAnalysisDto
{
    public long SegmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double AverageGrade { get; set; }
    public double MaximumGrade { get; set; }
    public double TotalElevationGain { get; set; }
    public string DifficultyLevel { get; set; } = string.Empty;
    public double EstimatedTimeMinutes { get; set; }
    public LeaderboardDto? Leaderboard { get; set; }
}

public class SegmentComparisonDto
{
    public long SegmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double AverageGrade { get; set; }
    public double ElevationGain { get; set; }
    public int AthleteCount { get; set; }
    public string DifficultyLevel { get; set; } = string.Empty;
}

public class ExploreSegmentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClimbCategory { get; set; } = string.Empty;
    public string ClimbCategoryDesc { get; set; } = string.Empty;
    public double AvgGrade { get; set; }
    public double Distance { get; set; }
    public double ElevDifference { get; set; }
    public int StarCount { get; set; }
    public List<double> StartLatlng { get; set; } = new();
    public List<double> EndLatlng { get; set; } = new();
    public string? Points { get; set; }
}
