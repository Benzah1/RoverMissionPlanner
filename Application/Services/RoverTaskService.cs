using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class RoverTaskService : IRoverTaskService
{
    // simulacion de persistencia en la memoria
    private readonly List<RoverTask> _tasks = new();

    public Task AddTask(string roverName, CreateRoverTaskDto dto)
    {
        var newStart = dto.StartsAt.GetValueOrDefault();
        var newEnd = newStart.AddMinutes(dto.DurationMinutes);

        // Verificacion de solapamiento
        var overlapping = _tasks.Any(t =>
            t.RoverName == roverName && TheyOverlap(newStart, newEnd, t.StartsAt, t.DurationMinutes)
        );

        if (overlapping)
            throw new InvalidOperationException("La tarea se solapa con otra que ya existe.");


        var normalizedRover = roverName.ToUpperInvariant();

        // Crear una tarea nueva 
        var newTask = new RoverTask
        {
            Id = Guid.NewGuid(),
            RoverName = normalizedRover,
            TaskType = dto.TaskType,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            StartsAt = dto.StartsAt.GetValueOrDefault(),
            DurationMinutes = dto.DurationMinutes,
            Status = Domain.Entities.TaskStatus.Planned
        };

        _tasks.Add(newTask);

        return Task.CompletedTask;
    }

    public Task<List<RoverTask>> GetTasksByDate(string roverName, DateTime date)
    {
        var tasks = _tasks
            .Where(t =>
                t.RoverName == roverName &&
                t.StartsAt.Date == date.Date
            )
            .OrderBy(t => t.StartsAt)
            .ToList();

        return Task.FromResult(tasks);
    }

    public Task<double> GetUtilization(string roverName, DateTime date)
    {
        var totalMinutes = 1440; // 24 horas
        var plannedMinutes = _tasks
            .Where(t => t.RoverName == roverName && t.StartsAt.Date == date.Date)
            .Sum(t => t.DurationMinutes);

        var percentage = (double)plannedMinutes / totalMinutes * 100;
        return Task.FromResult(Math.Round(percentage, 2));
    }





    private bool TheyOverlap(DateTime newStart, DateTime newEnd, DateTime eStart, int eDuration)
    {
        var eEnd = eStart.AddMinutes(eDuration);
        return newStart < eEnd && newEnd > eStart;
    }


}


