using HoL.Aplication.Interfaces.IDtos.IStatDtos.IStatReads;
using HoL.Aplication.Interfaces.IDtos.IStatDtos.IStatWrites;
using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.StatDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci statistiky postavy nebo entity.
    /// </summary>
    /// <remarks>
    /// StatDto představuje jednu z primárních statistik (STR, DEX, CON, INT, WIS, CHA) s její základní hodnotou,
    /// úpravami a finální vypočítanou hodnotou. Každá stat má také bonus modifikátor používaný pro hody.
    /// </remarks>
    /// <example>
    /// Příklady statistik:
    /// <code>
    /// var paladinCharisma = new StatDto
    /// {
    ///     Type = StatType.Charisma,
    ///     RawValue = 18,           
    ///     ValueAdjustment = 1,     
    ///     FinalValue = 19,         // 18 + 1 = 19
    ///     BonusAdjustment = 2      
    ///     FinalBonus = 6,          // from Table -> 19 -> (4+BonusAdjustment)
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="StatType"/>
    /// <seealso cref="StatDtoValidator"/>
    public class StatDto: IStatReadDto, IStatWriteDto
    {
        /// <summary>
        /// Typ statistiky (STR, DEX, CON, INT, WIS, CHA).
        /// </summary> 
        public StatType Type { get; set; }

        /// <summary>
        /// Základní hodnota statistiky ze stat rollu nebo point buy.
        /// </summary>
        public int RawValue { get; set; }

        /// <summary>
        /// Trvalé úpravy základní hodnoty z ras, levelů, itemů a permanent efektů.
        /// </summary
        public int ValueAdjustment { get; set; }

        /// <summary>
        /// Vysledna hodnota bonusu po zapocteni BonusAdjustmentu.
        /// </summary>
        public int FinalBonus { get; set; } 

        /// <summary>
        /// Dočasné úpravy bonus modifikátoru z buffs, debuffs a temporary efektů.
        /// </summary>
        public int BonusAdjustment { get; set; } 

        /// <summary>
        /// Finální vypočítaná hodnota statistiky (RawValue + ValueAdjustment).
        /// </summary>
        public int FinalValue { get; set; } 
    }
}
