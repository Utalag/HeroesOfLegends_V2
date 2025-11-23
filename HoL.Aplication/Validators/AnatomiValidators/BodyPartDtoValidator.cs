using FluentValidation;
using HoL.Aplication.DTOs.AnatomiDtos;

namespace HoL.Aplication.Validators.AnatomiValidators
{
    /// <summary>
    /// Validator pro BodyPartDto - validace části těla.
    /// Znovupoužitelný pro všechny entity s anatomií.
    /// </summary>
    public class BodyPartDtoValidator : AbstractValidator<BodyPartDto>
    {
        public BodyPartDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Body part name is required")
                .MaximumLength(100)
                .WithMessage("Body part name cannot exceed 100 characters");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid BodyPartType value");

            RuleFor(x => x.Count)
                .GreaterThan(0)
                .WithMessage("Body part count must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Body part count cannot exceed 100");

            RuleFor(x => x.Function)
                .MaximumLength(200)
                .WithMessage("Function description cannot exceed 200 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Function));

            RuleFor(x => x.Appearance)
                .MaximumLength(200)
                .WithMessage("Appearance description cannot exceed 200 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Appearance));

            // Vnořená validace Attack pokud existuje
            RuleFor(x => x.Attack)
                .SetValidator(new BodyPartAttackDtoValidator()!)
                .When(x => x.Attack != null);

            // Vnořená validace Defense pokud existuje
            RuleFor(x => x.Defense)
                .SetValidator(new BodyPartDefenseDtoValidator()!)
                .When(x => x.Defense != null);
        }
    }
}
