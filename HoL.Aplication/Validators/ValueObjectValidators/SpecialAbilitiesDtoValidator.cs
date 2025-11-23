using FluentValidation;
using HoL.Aplication.DTOs.ValueObjectDtos;

namespace HoL.Aplication.Validators.ValueObjectValidators
{
    /// <summary>
    /// Validator pro SpecialAbilitiesDto - validace speciálních schopností.
    /// Znovupoužitelný pro Race, Character, Item a další entity se schopnostmi.
    /// </summary>
    public class SpecialAbilitiesDtoValidator : AbstractValidator<SpecialAbilitiesDto>
    {
        public SpecialAbilitiesDtoValidator()
        {
            RuleFor(x => x.AbilityName)
                .NotEmpty()
                .WithMessage("Ability name is required")
                .MaximumLength(100)
                .WithMessage("Ability name cannot exceed 100 characters");

            RuleFor(x => x.AbilityDescription)
                .MaximumLength(500)
                .WithMessage("Ability description cannot exceed 500 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.AbilityDescription));
        }
    }
}
