using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.AnatomiDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci útočných schopností části těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// BodyPartAttackDto definuje jak část těla může být použita k útoku - damage, typ poškození a iniciativu.
    /// Používá se pro přirozené zbraně jako jsou drápy, zuby, rohy, ocasy, bodce nebo kopyto.
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklady útoků částí těla:
    /// <code>
    /// // Rozsah damage: 12-56
    /// var dragonBite = new BodyPartAttackDto
    /// {
    ///     DamageDice = new DiceDto { Quantity = 4, Sides = DiceType.D12, FinalBonus = 8 },
    ///     DamageType = DamageType.Piercing,
    ///     Initiative = 1
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="BodyPartDto"/>
    /// <seealso cref="DiceDto"/>
    /// <seealso cref="DamageType"/>
    /// <seealso cref="BodyPartAttackDtoValidator"/>
    public class BodyPartAttackDto
    {
        /// <summary>
        /// Kostky pro výpočet poškození tohoto útoku.
        /// </summary>
        public DiceDto DamageDice { get; set; } = new();

        /// <summary>
        /// Typ poškození způsobený tímto útokem.
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Iniciativa tohoto útoku v multi-attack sekvenci.
        /// </summary>
        /// <value>
        /// Rozsah: 0-100. Nižší číslo = útočí dříve.
        /// Pokud má entita více útoků, provádí se od nejnižší iniciativy k nejvyšší.
        /// </value>
        public int Initiative { get; set; } = 0;
    }
}
