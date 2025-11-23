using FluentValidation;
using HoL.Aplication.DTOs.ValueObjectDtos;

namespace HoL.Aplication.Validators.ValueObjectValidators
{
    /// <summary>
    /// Validator pro WeaponDto - validace zbraně.
    /// Znovupoužitelný pro Race, Character, Equipment a další.
    /// </summary>
    public class WeaponDtoValidator : AbstractValidator<WeaponDto>
    {
        public WeaponDtoValidator()
        {
            RuleFor(x => x.WeaponName)
                .NotEmpty()
                .WithMessage("Weapon name is required")
                .MaximumLength(100)
                .WithMessage("Weapon name cannot exceed 100 characters");
        }
    }
}
