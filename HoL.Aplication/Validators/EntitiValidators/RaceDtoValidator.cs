using FluentValidation;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Validators.ValueObjectValidators;
using HoL.Aplication.Validators.AnatomiValidators;
using HoL.Aplication.Validators.StatValidators;

namespace HoL.Aplication.Validators.EntitiValidators
{
    /// <summary>
    /// Validator pro RaceDto - obsahuje všechna validační pravidla pro entitu Race.
    /// Používá kompozici s jinými validátory pro znovupoužitelnost.
    /// </summary>
    public class RaceDtoValidator : AbstractValidator<RaceDto>
    {
        public RaceDtoValidator()
        {
            // === ID validace ===
            
            RuleFor(x => x.RaceId)
                .GreaterThan(0)
                .WithMessage("RaceId must be greater than 0 for update operation");

            // === Povinná pole ===

            RuleFor(x => x.RaceName)
                .NotEmpty()
                .WithMessage("RaceName is required")
                .MaximumLength(100)
                .WithMessage("RaceName cannot exceed 100 characters")
                .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\-']+$")
                .WithMessage("RaceName can only contain letters, numbers, spaces, hyphens and apostrophes");

            // === Popisné pole ===

            RuleFor(x => x.RaceDescription)
                .MaximumLength(1000)
                .WithMessage("RaceDescription cannot exceed 1000 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.RaceDescription));

            RuleFor(x => x.RaceHistory)
                .MaximumLength(5000)
                .WithMessage("RaceHistory cannot exceed 5000 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.RaceHistory));

            // === Numerické hodnoty s rozsahem ===

            RuleFor(x => x.BaseXP)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BaseXP must be non-negative")
                .LessThanOrEqualTo(100000)
                .WithMessage("BaseXP cannot exceed 100000");

            RuleFor(x => x.ZSM)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ZSM (Základní Společenská Míra) must be non-negative")
                .LessThanOrEqualTo(20)
                .WithMessage("ZSM cannot exceed 20");

            RuleFor(x => x.DomesticationValue)
                .GreaterThanOrEqualTo(0)
                .WithMessage("DomesticationValue must be non-negative")
                .LessThanOrEqualTo(100)
                .WithMessage("DomesticationValue cannot exceed 100");

            RuleFor(x => x.BaseInitiative)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BaseInitiative must be non-negative")
                .LessThanOrEqualTo(30)
                .WithMessage("BaseInitiative cannot exceed 30");

            // === Enum validace ===

            RuleFor(x => x.RaceCategory)
                .IsInEnum()
                .WithMessage("Invalid RaceCategory value");

            RuleFor(x => x.Conviction)
                .IsInEnum()
                .WithMessage("Invalid ConvictionType value");

            // === Kolekce ===

            RuleFor(x => x.RaceHierarchySystem)
                .NotNull()
                .WithMessage("RaceHierarchySystem cannot be null");

            RuleForEach(x => x.RaceHierarchySystem)
                .NotEmpty()
                .WithMessage("RaceHierarchySystem cannot contain empty strings")
                .MaximumLength(200)
                .WithMessage("Hierarchy item cannot exceed 200 characters")
                .When(x => x.RaceHierarchySystem != null);

            // === Mobility dictionary ===

            RuleFor(x => x.Mobility)
                .NotNull()
                .WithMessage("Mobility dictionary cannot be null");

            RuleFor(x => x.Mobility)
                .Must(m => m.All(kv => kv.Value >= 0))
                .WithMessage("All mobility values must be non-negative")
                .When(x => x.Mobility != null && x.Mobility.Any());

            // === Vnořené objekty - použití specializovaných validátorů ===

            RuleFor(x => x.SpecialAbilities)
                .NotNull()
                .WithMessage("SpecialAbilities collection cannot be null");

            RuleForEach(x => x.SpecialAbilities)
                .SetValidator(new SpecialAbilitiesDtoValidator())
                .When(x => x.SpecialAbilities != null && x.SpecialAbilities.Any());

            RuleFor(x => x.Treasure)
                .NotNull()
                .WithMessage("Treasure cannot be null")
                .SetValidator(new CurrencyDtoValidator()!);

            RuleFor(x => x.Vulnerability)
                .NotNull()
                .WithMessage("Vulnerability profile cannot be null")
                .SetValidator(new VulnerabilityProfilDtoValidator()!);

            RuleFor(x => x.FightingSpirit)
                .NotNull()
                .WithMessage("FightingSpirit cannot be null")
                .SetValidator(new FightingSpiritDtoValidator()!);

            RuleFor(x => x.RaceWeapon)
                .NotNull()
                .WithMessage("RaceWeapon cannot be null")
                .SetValidator(new WeaponDtoValidator()!);

            RuleFor(x => x.BodyDimensins)
                .NotNull()
                .WithMessage("Body dimensions (AnatomyProfile) cannot be null")
                .SetValidator(new AnatomyProfileDtoValidator()!);

            // === StatsPrimar dictionary s ValueRangeValidator ===

            RuleFor(x => x.StatsPrimar)
                .NotNull()
                .WithMessage("StatsPrimar dictionary cannot be null");

            RuleForEach(x => x.StatsPrimar.Values)
                .SetValidator(new ValueRangeDtoValidator())
                .When(x => x.StatsPrimar != null && x.StatsPrimar.Any());

            // === Custom business rules ===

            RuleFor(x => x)
                .Must(dto => dto.DomesticationValue == 0 || dto.BaseXP >= dto.DomesticationValue * 5)
                .WithMessage("BaseXP must be at least 5 times the DomesticationValue for domesticable races")
                .When(x => x.DomesticationValue > 0);
        }
    }
}
