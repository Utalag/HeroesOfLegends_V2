using HoL.Domain.Enums;

namespace HoL.Aplication.Interfaces.IDtos.IStatDtos.IStatWrites
{
    /// <summary>
    /// Interface pro ulozeni Statu
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
    ///     BodyPartCategory = BodyStat.Charisma,
    ///     RawValue = 18,           
    ///     ValueAdjustment = 1,     
    ///     FinalValue = 19,         // 18 + 1 = 19
    ///     BonusAdjustment = 2      
    ///     FinalBonus = 6,          // from Table -> 19 -> (4+BonusAdjustment)
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="BodyStat"/>
    /// <seealso cref="StatDtoValidator"/>
    internal interface IStatWriteDto
    {
        /// <summary>
        /// Typ statistiky (STR, DEX, CON, INT, WIS, CHA).
        /// </summary>
        BodyStat Type { get; set; }
        /// <summary>
        /// Trvalé úpravy základní hodnoty z ras, levelů, itemů a permanent efektů.
        /// </summary
        int ValueAdjustment { get; set; }
        /// <summary>
        /// Základní hodnota statistiky ze stat rollu nebo point buy.
        /// </summary>
        int RawValue { get; set; }
        /// <summary>
        /// Dočasné úpravy bonus modifikátoru z buffs, debuffs a temporary efektů.
        /// </summary>
        int BonusAdjustment { get; set; }

    }
}