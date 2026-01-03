using FluentValidation;
using HoL.Aplication.DTOs.ValueObjectDtos;

namespace HoL.Aplication.Validators.ValueObjectValidators
{
    /// <summary>
    /// Validator pro CurrencyDto - validace měnových hodnot.
    /// Znovupoužitelný ve všech DTOs které obsahují Treasure (Race, Character, Shop, atd.).
    /// </summary>
    public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
    {
        public CurrencyDtoValidator()
        {
            RuleFor(x => x.Gold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Gold must be non-negative")
                .When(x => x.Gold.HasValue);

            RuleFor(x => x.Silver)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Silver must be non-negative")
                .When(x => x.Silver.HasValue);

            RuleFor(x => x.Copper)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Copper must be non-negative")
                .When(x => x.Copper.HasValue);

            // Alespoň jedna měna musí být zadaná
            RuleFor(x => x)
                .Must(c => c.Gold.HasValue || c.Silver.HasValue || c.Copper.HasValue)
                .WithMessage("At least one currencyGroup value (Gold, Silver, or Copper) must be specified");
        }
    }
}
