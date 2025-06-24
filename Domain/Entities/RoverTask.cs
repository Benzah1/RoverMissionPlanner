namespace Domain.Entities;

public enum TaskType
{
    Drill,
    Sample,
    Photo,
    Charge
}

public enum TaskStatus
{
    Planned,
    InProgress,
    Completed,
    Aborted
}

public class RoverTask
{
    public Guid Id { get; set; }
    public string RoverName { get; set; } = string.Empty;
    public TaskType TaskType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime StartsAt { get; set; }
    public int DurationMinutes { get; set; }
    public TaskStatus Status { get; set; }
}
