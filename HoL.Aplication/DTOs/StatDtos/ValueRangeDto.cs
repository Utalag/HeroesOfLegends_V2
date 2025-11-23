using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.StatDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci rozsahu hodnot generovaných pomocí kostek.
    /// </summary>
    public class ValueRangeDto
    {
        /// <summary>
        /// Minimální možná hodnota (base value před hodem kostkou).
        /// </summary>
        public int Min { get; set; } = 0;

        /// <summary>
        /// Počet kostek k hodu pro určení hodnoty nad minimum.
        /// </summary>
        public int DiceCount { get; set; } = 1;

        /// <summary>
        /// Typ kostky (počet stran) použité pro hod.
        /// </summary>
        public DiceType DiceType { get; set; } = DiceType.D6;

        /// <summary>
        /// Maximální možná hodnota (Min + DiceCount × DiceType).
        /// </summary>
        public int Max { get; set; } = 6;
    }
}
