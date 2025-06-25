using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using API.Models;
using Prometheus;
using Application.Validators; 
using FluentValidation;       

namespace API.Controllers;

[ApiController]
[Route("rovers/{roverName}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IRoverTaskService _roverTaskService;

    // 👇 Métricas Prometheus
    private static readonly Counter CreateTaskCounter = Metrics
        .CreateCounter("create_task_requests_total", "Número total de llamadas a CreateTask");

    private static readonly Counter GetTasksCounter = Metrics
        .CreateCounter("get_tasks_requests_total", "Número total de llamadas a GetTasks");

    private static readonly Counter GetUtilizationCounter = Metrics
        .CreateCounter("get_utilization_requests_total", "Número total de llamadas a GetUtilization");

    public TasksController(IRoverTaskService roverTaskService)
    {
        _roverTaskService = roverTaskService;

        CreateTaskCounter.Inc(0);
        GetTasksCounter.Inc(0);
        GetUtilizationCounter.Inc(0);
    }

    // POST /rovers/{roverName}/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(string roverName, [FromBody] CreateRoverTaskRequest request)
    {
        CreateTaskCounter.Inc();

        var dto = new CreateRoverTaskDto
        {
            TaskType = request.TaskType,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            StartsAt = request.StartsAt,
            DurationMinutes = request.DurationMinutes
        };

        // Validación manual con FluentValidation
        var validator = new CreateRoverTaskDtoValidator();
        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(new { errors });
        }

        await _roverTaskService.AddTask(roverName, dto);
        return StatusCode(201);
    }

    // GET /rovers/{roverName}/tasks?date=YYYY-MM-DD
    [HttpGet]
    public async Task<IActionResult> GetTasks(string roverName, [FromQuery] DateTime date)
    {
        GetTasksCounter.Inc();

        var tasks = await _roverTaskService.GetTasksByDate(roverName, date);
        return Ok(tasks);
    }

    // GET /rovers/{roverName}/utilization?date=YYYY-MM-DD
    [HttpGet("/rovers/{roverName}/utilization")]
    public async Task<IActionResult> GetUtilization(string roverName, [FromQuery] DateTime date)
    {
        GetUtilizationCounter.Inc();

        var utilization = await _roverTaskService.GetUtilization(roverName, date);
        return Ok(new { utilizationPercentage = utilization });
    }
}
