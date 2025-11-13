namespace StravaConsumer.API.Models.Dto;

public class LeaderboardDto
{
    public long SegmentId { get; set; }
    public int EntryCount { get; set; }
    public List<LeaderboardEntryDto> Entries { get; set; } = new();
}

public class LeaderboardEntryDto
{
    public string AthleteName { get; set; } = string.Empty;
    public long AthleteId { get; set; }
    public int Rank { get; set; }
    public int ElapsedTime { get; set; }
    public int MovingTime { get; set; }
    public DateTime StartDate { get; set; }
    public long ActivityId { get; set; }
    public long EffortId { get; set; }
}
