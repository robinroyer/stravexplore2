namespace StravaConsumer.API.Models.Domain;

public class Route
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long AthleteId { get; set; }
    public double Distance { get; set; }
    public double ElevationGain { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool Private { get; set; }
    public int Starred { get; set; }
    public DateTime Timestamp { get; set; }
}
