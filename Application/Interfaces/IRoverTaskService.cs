using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces;

public interface IRoverTaskService
{
    Task AddTask(string roverName, CreateRoverTaskDto dto);
    Task<List<RoverTask>> GetTasksByDate(string roverName, DateTime date);
    Task<double> GetUtilization(string roverName, DateTime date);
}
