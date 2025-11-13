namespace StravaConsumer.API.Models.Dto;

public class TrendingSegmentDto
{
    public long SegmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double AverageGrade { get; set; }
    public int StarCount { get; set; }
    public int EffortCount { get; set; }
    public int AthleteCount { get; set; }
    public double TrendScore { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class SegmentTrendDataDto
{
    public long SegmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<TrendDataPointDto> TrendData { get; set; } = new();
    public double OverallTrend { get; set; }
}

public class TrendDataPointDto
{
    public DateTime Date { get; set; }
    public int EffortCount { get; set; }
    public int AthleteCount { get; set; }
}

public class EventSegmentDto
{
    public long SegmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public int StarCount { get; set; }
    public int EstimatedParticipants { get; set; }
    public int PrCount { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public List<double> StartLocation { get; set; } = new();
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
