using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HoL.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HoL.Domain.Helpers
{
    /// <summary>
    /// Value object reprezentující kostky používané při útoku nebo testu.
    /// </summary>
    [Owned]
    public class Dice
    {
        /// <summary>
        /// Počet kostek.
        /// </summary>
        public int Count { get; private set; } = 1;

        /// <summary>
        /// Typ stěn kostek.
        /// </summary>
        public DiceType Sides { get; private set; } = DiceType.D6;

        /// <summary>
        /// Bonus, který se přičte k hodu.
        /// </summary>
        public int Bonus { get; private set; } = 0;

        /// <summary>
        /// Vytvoří sadu kostek.
        /// </summary>
        /// <param name="count">Počet kostek.</param>
        /// <param name="sides">Typ stěn.</param>
        /// <param name="bonus">Bonus k součtu.</param>
        public Dice(int count, DiceType sides, int bonus)
        {
            Count = count;
            Sides = sides;
            Bonus = bonus;
        }

        /// <summary>
        /// Nastaví počet kostek.
        /// </summary>
        /// <param name="count">Počet kostek.</param>
        public Dice SetQuantity(int count)
        {
            Count = count;
            return this;
        }

        /// <summary>
        /// Nastaví typ stěn.
        /// </summary>
        /// <param name="sides">Typ kostky.</param>
        public Dice SetSides(DiceType sides)
        {
            Sides = sides;
            return this;
        }

        /// <summary>
        /// Nastaví bonus přičítaný k výsledku hodu.
        /// </summary>
        /// <param name="bonus">Bonusová hodnota.</param>
        public Dice SetBonus(int bonus)
        {
            Bonus = bonus;
            return this;
        }
    }
}
