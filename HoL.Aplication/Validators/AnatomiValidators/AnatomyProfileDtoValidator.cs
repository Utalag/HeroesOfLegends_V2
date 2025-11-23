using FluentValidation;
using HoL.Aplication.DTOs.AnatomiDtos;

namespace HoL.Aplication.Validators.AnatomiValidators
{
    /// <summary>
    /// FluentValidation validator pro <see cref="AnatomyProfileDto"/>.
    /// Zajišťuje validitu anatomického profilu včetně rozměrů, věku a částí těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Validator kontroluje:
    /// <list type="bullet">
    /// <item>
    ///     <description><c>RaceSize</c> musí být platná hodnota enumu</description>
    /// </item>
    /// <item>
    ///     <description><c>WeightMin</c> >= 0 a <c>WeightMax</c> > <c>WeightMin</c></description>
    /// </item>
    /// <item>
    ///     <description><c>BodyHeightMin</c> >= 0 a <c>BodyHeightMax</c> > <c>BodyHeightMin</c></description>
    /// </item>
    /// <item>
    ///     <description><c>HeightMin</c> >= 0 a <c>HeightMax</c> > <c>HeightMin</c></description>
    /// </item>
    /// <item>
    ///     <description><c>MaxAge</c> musí být 1-10000 let</description>
    /// </item>
    /// <item>
    ///     <description><c>BodyParts</c> kolekce muze být null a každá část je validována <see cref="BodyPartDtoValidator"/></description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Tento validator je znovupoužitelný pro všechny entity s anatomií (Race, Monster, NPC).
    /// </para>
    /// </remarks>
    /// <seealso cref="AnatomyProfileDto"/>
    /// <seealso cref="BodyPartDtoValidator"/>
    /// <seealso cref="RaceDtoValidator"/>
    public class AnatomyProfileDtoValidator : AbstractValidator<AnatomyProfileDto>
    {
        /// <summary>
        /// Inicializuje novou instanci <see cref="AnatomyProfileDtoValidator"/>.
        /// Definuje všechna validační pravidla pro anatomický profil.
        /// </summary>
        public AnatomyProfileDtoValidator()
        {
            // === Velikost rasy ===
            
            RuleFor(x => x.RaceSize)
                .IsInEnum()
                .WithMessage("Invalid RaceSize value");

            // === Váhové rozsahy ===
            
            RuleFor(x => x.WeightMin)
                .GreaterThanOrEqualTo(0)
                .WithMessage("WeightMin must be non-negative");

            RuleFor(x => x.WeightMax)
                .GreaterThan(x => x.WeightMin)
                .WithMessage("WeightMax must be greater than WeightMin");

            // === Výškové rozsahy těla (kohoutek/ramena) ===
            
            RuleFor(x => x.BodyHeightMin)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BodyHeightMin must be non-negative");

            RuleFor(x => x.BodyHeightMax)
                .GreaterThan(x => x.BodyHeightMin)
                .WithMessage("BodyHeightMax must be greater than BodyHeightMin");

            // === Celkové výškové rozsahy ===
            
            RuleFor(x => x.HeightMin)
                .GreaterThanOrEqualTo(0)
                .WithMessage("HeightMin must be non-negative");

            RuleFor(x => x.HeightMax)
                .GreaterThan(x => x.HeightMin)
                .WithMessage("HeightMax must be greater than HeightMin");

            // === Maximální věk ===
            
            RuleFor(x => x.MaxAge)
                .GreaterThan(0)
                .WithMessage("MaxAge must be greater than 0")
                .LessThanOrEqualTo(10000)
                .WithMessage("MaxAge cannot exceed 10000 years");

            // === Části těla (vnořená validace) ===

            if (RuleFor(x => x.BodyParts) != null)
            {
                RuleForEach(x => x.BodyParts)
                .SetValidator(new BodyPartDtoValidator());
            };
                    

        }
    }
}
