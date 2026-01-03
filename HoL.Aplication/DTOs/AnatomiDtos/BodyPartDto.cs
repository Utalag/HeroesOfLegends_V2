using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.AnatomiDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci části těla entity s jejími vlastnostmi.
    /// </summary>
    /// <remarks>
    /// <para>
    /// BodyPartDto představuje anatomickou část těla s volitelně definovanými útočnými nebo obrannými schopnostmi.
    /// Každá část může mít různé funkce (vidění, let, útok, pohyb) a vlastnosti (vzhled, magické efekty).
    /// </para>
    /// <para>
    /// Systém částí těla umožňuje:
    /// <list type="bullet">
    /// <item><description>Definovat komplexní anatomii (hlava, končetiny, křídla, ocasy, chapadla)</description></item>
    /// <item><description>Přiřadit útočné schopnosti (drápy, zuby, bodnutí ocasem)</description></item>
    /// <item><description>Definovat obranu (krunýř, šupiny, kůže)</description></item>
    /// <item><description>Sledovat zranění specifických částí těla</description></item>
    /// <item><description>Simulovat ztrátu končetin a jejich dopad</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklady různých částí těla:
    /// <code>
    /// var scorpionTail = new BodyPartDto
    /// {
    ///     Name = "Jedový ocas",
    ///     BodyPartCategory = BodyPartType.Tail,
    ///     Quantity = 1,
    ///     Function = "Útok s jedem",
    ///     IsMagical = true,
    ///     Attack = new BodyPartAttackDto
    ///     {
    ///         DamageDice = new DiceDto { Quantity = 1, Sides = DiceType.D8, FinalBonus = 2 },
    ///         DamageType = DamageType.Poison,
    ///         Initiative = 3
    ///     }
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="BodyPartType"/>
    /// <seealso cref="BodyPartAttackDto"/>
    /// <seealso cref="BodyPartDefenseDto"/>
    /// <seealso cref="BodyPartDtoValidator"/>
    public class BodyPartDto
    {
        /// <summary>
        /// Unikátní název části těla.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Typ části těla určující její základní kategorii.
        /// </summary>
        public BodyPartType Type { get; set; }

        /// <summary>
        /// Počet těchto částí na těle entity.
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// Popis funkce části těla v herní mechanice.
        /// </summary>
        public string Function { get; set; } = string.Empty;

        /// <summary>
        /// Útočné schopnosti této části těla, pokud existují.
        /// </summary>
        public BodyPartAttackDto? Attack { get; set; }

        /// <summary>
        /// Obranné vlastnosti této části těla, pokud existují.
        /// </summary>
        public BodyPartDefenseDto? Defense { get; set; }

        /// <summary>
        /// Volitelný vizuální popis části těla.
        /// </summary>
        public string Appearance { get; set; } = string.Empty;

        /// <summary>
        /// Indikuje zda je část těla magická nebo má nadpřirozené vlastnosti.
        /// </summary>
        public bool IsMagical { get; set; } = false;
    }
}

