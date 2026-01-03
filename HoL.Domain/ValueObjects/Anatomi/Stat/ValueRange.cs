using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;

namespace HoL.Domain.ValueObjects.Anatomi.Stat
{

    // Nová implementace: Min + DiceCount model (Max je odvozené)
    [DebuggerDisplay("{Min}-{Max}")]
    public class ValueRange
    {
        /// <summary>
        /// Minimální možná hodnota (např. minimální součet hodů).
        /// </summary>
        public int Min { get; private set; }

        /// <summary>
        /// Počet hodů (počet kostek), např. 2 znamená 2 hodů danou kostkou.
        /// </summary>
        public int DiceCount { get; private set; }

        /// <summary>
        /// Typ kostky (D6, D20 ...).
        /// </summary>
        public DiceType DiceType { get; private set; }

        /// <summary>
        /// Max je odvozené tak, aby span = Max - Min = DiceCount * (sides - 1)
        /// </summary>
        public int Max => Min + DiceCount * ((int)DiceType - 1);


        public ValueRange(int min, int diceCount, DiceType diceType)
        {
            if (diceCount < 1)
                throw new ArgumentException("Počet hodů musí být alespoň 1.", nameof(diceCount));
            Min = min;
            DiceCount = diceCount;
            DiceType = diceType;
        }
        public ValueRange SetMin(int min)
        {
            Min = min;
            return this;
        }
        public ValueRange SetDiceCount(int diceCount)
        {
            if (diceCount < 1)
                throw new ArgumentException("Počet hodů musí být alespoň 1.", nameof(diceCount));
            DiceCount = diceCount;
            return this;
        }
        public ValueRange SetDiceType(DiceType diceType)
        {
            DiceType = diceType;
            return this;
        }
    }
}

