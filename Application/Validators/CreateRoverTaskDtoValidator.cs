using FluentValidation;
using Application.DTOs;

namespace Application.Validators;

public class CreateRoverTaskDtoValidator : AbstractValidator<CreateRoverTaskDto>
{
    public CreateRoverTaskDtoValidator()
    {
        RuleFor(x => x.TaskType)
            .IsInEnum().WithMessage("El tipo de tarea es inválido.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("La duración debe ser mayor a 0.");
    }
}
