using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.CreatedRace
{
    public class CreatedRaceValidator : AbstractValidator<CreatedRaceCommand>
    {
        public CreatedRaceValidator()
        {
            // Validace že RaceDto není null
            RuleFor(x => x.RaceDto)
                .NotNull()
                .WithMessage("RaceDto is required");
            // Vnořená validace RaceDto pomocí refaktorovaného validátoru
        }
    }
}
