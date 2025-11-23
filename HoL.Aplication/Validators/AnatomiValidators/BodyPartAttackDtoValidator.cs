using FluentValidation;
using HoL.Aplication.DTOs.AnatomiDtos;

namespace HoL.Aplication.Validators.AnatomiValidators
{
    /// <summary>
    /// Validator pro BodyPartAttackDto - validace útočných schopností části těla.
    /// Znovupoužitelný pro všechny entity s útočnými tělesnými částmi.
    /// </summary>
    public class BodyPartAttackDtoValidator : AbstractValidator<BodyPartAttackDto>
    {
        public BodyPartAttackDtoValidator()
        {
            RuleFor(x => x.DamageType)
                .IsInEnum()
                .WithMessage("Invalid DamageType value");

            RuleFor(x => x.Initiative)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Initiative must be non-negative")
                .LessThanOrEqualTo(100)
                .WithMessage("Initiative cannot exceed 100");

            // Validace DamageDice
            RuleFor(x => x.DamageDice)
                .NotNull()
                .WithMessage("DamageDice cannot be null")
                .SetValidator(new DiceDtoValidator()!);
        }
    }
}
