using Application.Interfaces;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Validators;
using Prometheus;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<CreateRoverTaskDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Rover service
builder.Services.AddSingleton<IRoverTaskService, RoverTaskService>();

var app = builder.Build();

// Prometheus
app.UseHttpMetrics(); // Captura métricas HTTP (por endpoint, método, etc.)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseMiddleware<API.Middleware.ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

// 🔥 Esta línea es clave
app.UseRouting();

app.UseAuthorization();

// 👇 Estas deben ir después del UseRouting
app.MapControllers();
app.MapMetrics(); // Exponer /metrics

app.Run();
