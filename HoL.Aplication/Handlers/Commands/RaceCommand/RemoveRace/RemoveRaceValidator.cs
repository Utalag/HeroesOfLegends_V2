using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.RemoveRace
{
    public class RemoveRaceValidator : AbstractValidator<RemoveRaceCommand>
    {
        public RemoveRaceValidator()
        {
            // Validace že RaceDto není null
            RuleFor(x => x.RaceDto)
                .NotNull()
                .WithMessage("RaceDto is required");
        }
    }
}
