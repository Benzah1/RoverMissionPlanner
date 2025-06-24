using Application.Interfaces;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Validators;
using System.Text.Json.Serialization; // 👈 Necesario para JsonStringEnumConverter

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializar enums como strings ("Drill", "Photo", etc.)
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

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Angular
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Registrar servicio Rover
builder.Services.AddSingleton<IRoverTaskService, RoverTaskService>();

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar CORS antes de cualquier middleware que procese la request
app.UseCors();

app.UseMiddleware<API.Middleware.ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
