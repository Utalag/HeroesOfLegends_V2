using FluentValidation;
using HoL.Aplication.DTOs.AnatomiDtos;

namespace HoL.Aplication.Validators.AnatomiValidators
{
    /// <summary>
    /// Validator pro DiceDto - validace hodu kostkou.
    /// Znovupoužitelný pro všechny entity používající náhodné hody (damage, stats, atd.).
    /// </summary>
    public class DiceDtoValidator : AbstractValidator<DiceDto>
    {
        public DiceDtoValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .WithMessage("Dice count must be greater than 0")
                .LessThanOrEqualTo(20)
                .WithMessage("Dice count cannot exceed 20");

            RuleFor(x => x.Sides)
                .IsInEnum()
                .WithMessage("Invalid dice type");

            RuleFor(x => x.Bonus)
                .GreaterThanOrEqualTo(-50)
                .WithMessage("Dice bonus cannot be less than -50")
                .LessThanOrEqualTo(100)
                .WithMessage("Dice bonus cannot exceed 100");
        }
    }
}
