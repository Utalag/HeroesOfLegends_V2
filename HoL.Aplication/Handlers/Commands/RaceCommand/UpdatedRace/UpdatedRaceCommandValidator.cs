using FluentValidation;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Validators.EntitiValidators;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace
{
    /// <summary>
    /// Validator pro UpdatedRaceCommand pomocí FluentValidation.
    /// Používá refaktorované validátory ze složky Validators/ se stejnou strukturou jako DTOs/.
    /// </summary>
    public class UpdatedRaceCommandValidator : AbstractValidator<UpdatedRaceCommand>
    {
        public UpdatedRaceCommandValidator()
        {
            // Validace že RaceDto není null
            RuleFor(x => x.RaceDto)
                .NotNull()
                .WithMessage("RaceDto is required");

            // Vnořená validace RaceDto pomocí refaktorovaného validátoru
            RuleFor(x => x.RaceDto)
                .SetValidator(new RaceDtoValidator()!)
                .When(x => x.RaceDto != null);
        }
    }
}
