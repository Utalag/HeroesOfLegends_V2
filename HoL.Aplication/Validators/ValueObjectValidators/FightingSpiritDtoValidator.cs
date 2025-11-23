using FluentValidation;
using HoL.Aplication.DTOs.ValueObjectDtos;

namespace HoL.Aplication.Validators.ValueObjectValidators
{
    /// <summary>
    /// Validator pro FightingSpiritDto - validace bojového ducha.
    /// Znovupoužitelný pro Race, NPC, Monster a další bojující entity.
    /// </summary>
    public class FightingSpiritDtoValidator : AbstractValidator<FightingSpiritDto>
    {
        public FightingSpiritDtoValidator()
        {
            RuleFor(x => x.DangerNumber)
                .GreaterThanOrEqualTo(0)
                .WithMessage("DangerNumber must be non-negative")
                .LessThanOrEqualTo(20)
                .WithMessage("DangerNumber cannot exceed 20");
        }
    }
}
