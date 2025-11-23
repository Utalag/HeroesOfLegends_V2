using HoL.Domain.Enums;

namespace HoL.Aplication.Interfaces.IDtos.IStatDtos.IStatReads
{
    /// <summary>
    /// Interface pro zobrazeni Statu
    /// </summary>
    /// <remarks>
    /// StatDto představuje jednu z primárních statistik (STR, DEX, CON, INT, WIS, CHA) s její základní hodnotou,
    /// úpravami a finální vypočítanou hodnotou. Každá stat má také bonus modifikátor používaný pro hody.
    /// </remarks>

    /// <seealso cref="StatType"/>
    /// <seealso cref="StatDtoValidator"/>/// <summary>
    /// Interface pro zobrazeni Statu
    /// </summary>
    
    /// <remarks>
    /// StatDto představuje jednu z primárních statistik (STR, DEX, CON, INT, WIS, CHA) s její základní hodnotou,
    /// úpravami a finální vypočítanou hodnotou. Každá stat má také bonus modifikátor používaný pro hody.
    /// </remarks>

    /// <seealso cref="StatType"/>
    /// <seealso cref="StatDtoValidator"/>
    public interface IStatReadDto
    {
        /// <summary>
        /// Typ statistiky (STR, DEX, CON, INT, WIS, CHA).
        /// </summary>
        StatType Type { get; }
        /// <summary>
        /// Trvalé úpravy základní hodnoty z ras, levelů, itemů a permanent efektů.
        /// </summary
        int ValueAdjustment { get; }
        /// <summary>
        /// Základní hodnota statistiky ze stat rollu nebo point buy.
        /// </summary>
        int RawValue { get; }
        /// <summary>
        /// Vysledna hodnota bonusu po zapocteni BonusAdjustmentu.
        /// </summary>
        int FinalBonus { get; }
        /// <summary>
        /// Dočasné úpravy bonus modifikátoru z buffs, debuffs a temporary efektů.
        /// </summary>
        int BonusAdjustment { get;}
        /// <summary>
        /// Finální vypočítaná hodnota statistiky (RawValue + ValueAdjustment).
        /// </summary>
        int FinalValue { get; }
    }
}