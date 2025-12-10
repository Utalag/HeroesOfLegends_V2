using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;

namespace HoL.Domain.ValueObjects.Anatomi.Stat
{

    [ComplexType]
    // Nová implementace: Min + DiceCount model (Max je odvozené)
    public class ValueRange
    {
        /// <summary>
        /// Minimální možná hodnota (např. minimální součet hodů).
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Počet hodů (počet kostek), např. 2 znamená 2 hodů danou kostkou.
        /// </summary>
        public int DiceCount { get; set; }

        /// <summary>
        /// Typ kostky (D6, D20 ...).
        /// </summary>
        public DiceType DiceType { get; set; }

        /// <summary>
        /// Max je odvozené tak, aby span = Max - Min = DiceCount * (sides - 1)
        /// </summary>
        public int Max => Min + DiceCount * ((int)DiceType - 1);
    }
}

