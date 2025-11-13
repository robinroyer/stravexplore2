namespace StravaConsumer.API.Models.Domain;

public class Segment
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
    public int StarCount { get; set; }
    public int EffortCount { get; set; }
    public int AthleteCount { get; set; }
}
