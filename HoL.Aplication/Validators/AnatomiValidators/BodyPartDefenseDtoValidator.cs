using FluentValidation;
using HoL.Aplication.DTOs.AnatomiDtos;

namespace HoL.Aplication.Validators.AnatomiValidators
{
    /// <summary>
    /// Validator pro BodyPartDefenseDto - validace obranných vlastností části těla.
    /// Znovupoužitelný pro všechny entity s obrannými tělesnými částmi.
    /// </summary>
    public class BodyPartDefenseDtoValidator : AbstractValidator<BodyPartDefenseDto>
    {
        public BodyPartDefenseDtoValidator()
        {
            RuleFor(x => x.ArmorValue)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ArmorValue must be non-negative")
                .LessThanOrEqualTo(50)
                .WithMessage("ArmorValue cannot exceed 50");
        }
    }
}
