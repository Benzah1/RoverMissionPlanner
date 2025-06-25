using Xunit;
using Application.Services;
using Application.DTOs;
using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace Application.Tests.Services;

public class RoverTaskServiceTests
{
    [Fact]
    public async Task AddTask_Must_Add_Task()
    {
        // Arrange
        var service = new RoverTaskService();
        var dto = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Drill,
            Latitude = 1.0,
            Longitude = 1.0,
            StartsAt = DateTime.UtcNow,
            DurationMinutes = 60
        };

        // Act
        Func<Task> act = () => service.AddTask("spirit", dto);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task AddTask_Overlay_Must_Be_Activated()
    {
        // Arrange
        var service = new RoverTaskService();
        var now = DateTime.UtcNow;

        var task1 = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Sample,
            Latitude = 0,
            Longitude = 0,
            StartsAt = now,
            DurationMinutes = 60
        };

        var task2 = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Photo,
            Latitude = 0,
            Longitude = 0,
            StartsAt = now.AddMinutes(30), // Se solapa con task1
            DurationMinutes = 30
        };

        await service.AddTask("spirit", task1);

        // Act
        Func<Task> act = () => service.AddTask("spirit", task2);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("La tarea se solapa con otra que ya existe.");
    }

    [Fact]
    public async Task GetTasksByDate_Must_Return_Tasks_by_Specific_Date()
    {
        var service = new RoverTaskService();
        var date = new DateTime(2025, 6, 24);

        var task = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Charge,
            Latitude = 0,
            Longitude = 0,
            StartsAt = date.AddHours(12),
            DurationMinutes = 45
        };

        await service.AddTask("CURIOSITY", task);

        var result = await service.GetTasksByDate("CURIOSITY", date);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetUtilization_Must_Calculate_Percentage()
    {
        var service = new RoverTaskService();
        var date = new DateTime(2025, 6, 24);

        var task = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Photo,
            Latitude = 0,
            Longitude = 0,
            StartsAt = date.AddHours(6),
            DurationMinutes = 144 // 10%
        };

        await service.AddTask("PERSEVERANCE", task);

        var utilization = await service.GetUtilization("PERSEVERANCE", date);

        utilization.Should().Be(10.0);
    }

    [Fact]
    public async Task AddTask_the_task_must_start_after_the_previous_one_finishes()
    {
        // Arrange
        var service = new RoverTaskService();
        var now = DateTime.UtcNow;

        var task1 = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Drill,
            Latitude = 0,
            Longitude = 0,
            StartsAt = now,
            DurationMinutes = 60
        };

        var task2 = new CreateRoverTaskDto
        {
            TaskType = Domain.Entities.TaskType.Sample,
            Latitude = 0,
            Longitude = 0,
            StartsAt = now.AddMinutes(60), // empieza justo al terminar task1
            DurationMinutes = 30
        };

        // Act
        await service.AddTask("spirit", task1);
        Func<Task> act = () => service.AddTask("spirit", task2);

        // Assert
        await act.Should().NotThrowAsync();
    }

}
