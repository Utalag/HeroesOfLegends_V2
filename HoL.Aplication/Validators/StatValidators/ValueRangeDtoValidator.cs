using FluentValidation;
using HoL.Aplication.DTOs.StatDtos;

namespace HoL.Aplication.Validators.StatValidators
{
    /// <summary>
    /// Validator pro ValueRangeDto - validace rozsahu hodnot pro statistiky.
    /// Znovupoužitelný pro všechny DTOs používající rozsahy s kostkami (HP, stats, damage, atd.).
    /// </summary>
    public class ValueRangeDtoValidator : AbstractValidator<ValueRangeDto>
    {
        public ValueRangeDtoValidator()
        {
            RuleFor(x => x.Min)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum value must be non-negative");

            RuleFor(x => x.DiceCount)
                .GreaterThan(0)
                .WithMessage("Dice count must be greater than 0")
                .LessThanOrEqualTo(10)
                .WithMessage("Dice count cannot exceed 10");

            RuleFor(x => x.DiceType)
                .IsInEnum()
                .WithMessage("Invalid dice type");

            RuleFor(x => x.Max)
                .GreaterThan(x => x.Min)
                .WithMessage("Maximum must be greater than minimum");

            // Custom validace: Max by měl odpovídat vzorci Min + DiceCount * (DiceType - 1)
            RuleFor(x => x)
                .Must(v => v.Max == v.Min + v.DiceCount * ((int)v.DiceType - 1))
                .WithMessage("Max value must equal Min + DiceCount * (DiceType - 1)")
                .When(x => x.DiceCount > 0 && x.Min >= 0);
        }
    }
}
