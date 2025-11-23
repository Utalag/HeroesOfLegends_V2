using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.AnatomiDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci hodu kostkou v herním systému.
    /// </summary>
    public class DiceDto
    {
        /// <summary>
        /// Počet kostek k hodu.
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// Typ kostky (počet stran).
        /// </summary>
        public DiceType Sides { get; set; } = DiceType.D6;

        /// <summary>
        /// Pevný bonus nebo penalizace přidaná k součtu hodů.
        /// </summary>
        public int Bonus { get; set; } = 0;
    }
}
