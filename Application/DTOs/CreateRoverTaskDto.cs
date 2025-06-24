using Domain.Entities;


namespace Application.DTOs;

public class CreateRoverTaskDto
{
    public TaskType TaskType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? StartsAt { get; set; }
    public int DurationMinutes { get; set; }
}




