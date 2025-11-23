using FluentValidation;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceById
{
    /// <summary>
    /// FluentValidation validator pro <see cref="GetRaceByIdQuery"/>.
    /// Automaticky se spouští přes ValidationBehavior PŘED GetRaceByIdQueryHandler.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Validuje že:
    /// <list type="bullet">
    /// <item><description>Id je > 0 (validní ID)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Pokud validace selže, vyhodí <c>ValidationException</c> a handler se NESPUSTÍ.
    /// </para>
    /// <para>
    /// Neprovádí se kontrola existence v databázi - to je zodpovědnost handleru.
    /// Validator kontroluje pouze formát/rozsah ID.
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklady validace:
    /// <code>
    /// var validator = new GetRaceByIdQueryValidator();
    /// 
    /// // ✅ Validní
    /// var validQuery = new GetRaceByIdQuery(1);
    /// validator.Validate(validQuery).IsValid; // true
    /// 
    /// // ❌ Nevalidní
    /// var invalidQuery = new GetRaceByIdQuery(0);
    /// validator.Validate(invalidQuery).IsValid; // false
    /// 
    /// var negativeQuery = new GetRaceByIdQuery(-5);
    /// validator.Validate(negativeQuery).IsValid; // false
    /// </code>
    /// </example>
    /// <seealso cref="GetRaceByIdQuery"/>
    /// <seealso cref="GetRaceByIdQueryHandler"/>
    public class GetRaceByIdQueryValidator : AbstractValidator<GetRaceByIdQuery>
    {
        /// <summary>
        /// Inicializuje novou instanci <see cref="GetRaceByIdQueryValidator"/>.
        /// Definuje validační pravidla pro Id.
        /// </summary>
        public GetRaceByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0");
        }
    }
}
