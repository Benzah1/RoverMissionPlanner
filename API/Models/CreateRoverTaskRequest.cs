using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Models;

public class CreateRoverTaskRequest
{
    [SwaggerSchema("Tipo de tarea", Description = "Drill = 0, Sample = 1, Photo = 2, Charge = 3")]
    public TaskType TaskType { get; set; }

    [SwaggerSchema("Latitud (-90 a 90)")]
    public double Latitude { get; set; }

    [SwaggerSchema("Longitud (-180 a 180)")]
    public double Longitude { get; set; }

    [SwaggerSchema("Fecha y hora de inicio en UTC")]
    public DateTime? StartsAt { get; set; }

    [SwaggerSchema("Duración en minutos (>0)")]
    public int DurationMinutes { get; set; }
}
