using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using API.Models;

namespace API.Controllers;

[ApiController]
[Route("rovers/{roverName}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IRoverTaskService _roverTaskService;

    public TasksController(IRoverTaskService roverTaskService)
    {
        _roverTaskService = roverTaskService;
    }

    // POST /rovers/{roverName}/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(string roverName, [FromBody] CreateRoverTaskRequest request)
    {

        var dto = new CreateRoverTaskDto
        {
            TaskType = request.TaskType,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            StartsAt = request.StartsAt,
            DurationMinutes = request.DurationMinutes
        };
        
        await _roverTaskService.AddTask(roverName, dto);
        return StatusCode(201); // Created
        
        
    }

    // GET /rovers/{roverName}/tasks?date=YYYY-MM-DD
    [HttpGet]
    public async Task<IActionResult> GetTasks(string roverName, [FromQuery] DateTime date)
    {
        var tasks = await _roverTaskService.GetTasksByDate(roverName, date);
        return Ok(tasks);
    }

    // GET /rovers/{roverName}/utilization?date=YYYY-MM-DD
    [HttpGet("/rovers/{roverName}/utilization")]
    public async Task<IActionResult> GetUtilization(string roverName, [FromQuery] DateTime date)
    {
        var utilization = await _roverTaskService.GetUtilization(roverName, date);
        return Ok(new { utilizationPercentage = utilization });
    }
}
