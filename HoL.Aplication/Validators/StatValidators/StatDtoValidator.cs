using FluentValidation;
using HoL.Aplication.DTOs.StatDtos;

namespace HoL.Aplication.Validators.StatValidators
{
    /// <summary>
    /// Validator pro StatDto - validace základní statistiky postavy/rasy.
    /// Znovupoužitelný pro Character, Race, NPC a další entity se statistikami.
    /// </summary>
    public class StatDtoValidator : AbstractValidator<StatDto>
    {
        public StatDtoValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid BodyStat value");

            RuleFor(x => x.RawValue)
                .GreaterThanOrEqualTo(0)
                .WithMessage("RawValue must be non-negative")
                .LessThanOrEqualTo(100)
                .WithMessage("RawValue cannot exceed 100");

            RuleFor(x => x.ValueAdjustment)
                .GreaterThanOrEqualTo(-50)
                .WithMessage("ValueAdjustment cannot be less than -50")
                .LessThanOrEqualTo(50)
                .WithMessage("ValueAdjustment cannot exceed 50");

            RuleFor(x => x.BonusAdjustment)
                .GreaterThanOrEqualTo(-20)
                .WithMessage("BonusAdjustment cannot be less than -20")
                .LessThanOrEqualTo(20)
                .WithMessage("BonusAdjustment cannot exceed 20");

            RuleFor(x => x.FinalValue)
                .GreaterThanOrEqualTo(0)
                .WithMessage("FinalValue must be non-negative")
                .LessThanOrEqualTo(150)
                .WithMessage("FinalValue cannot exceed 150");

            // Konzistence: FinalValue by měl být RawValue + ValueAdjustment
            RuleFor(x => x)
                .Must(s => s.FinalValue == s.RawValue + s.ValueAdjustment)
                .WithMessage("FinalValue must equal RawValue + ValueAdjustment")
                .When(x => x.RawValue >= 0);
        }
    }
}
