using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using HoL.Domain.Interfaces.Read_Interfaces;
using HoL.Domain.Interfaces.Write_Interaces;

namespace HoL.Domain.ValueObjects.Anatomi.Stat
{
    [ComplexType]
    public class Stat : IStatRead, IStatWrite
    {
        /// <summary>
        /// Base statistic type
        /// </summary>
        public BodyStat Type { get; set; }
        /// <summary>
        /// Hlavní hodnota statistiky bez úprav
        /// </summary>
        public int RawValue { get; set; }
        /// <summary>
        /// Základní úprava RawValue (např. rasová úprava)
        /// </summary>
        public int ValueAdjustment { get; set; } = 0;
        /// <summary>
        /// Úprava bonusu (např. kouzelnické předměty)
        /// </summary>
        public int BonusAdjustment { get; set; } = 0;
        /// <summary>
        /// Bonus vycházející z RawValue
        /// </summary>
        public int RawBonus => StatModifierTable.GetModifier(RawValue);
        /// <summary>
        /// Výseldná hodnota statistiky po úpravách
        /// </summary>
        public int FinalValue { get => RawValue + ValueAdjustment; }
        /// <summary>
        /// Výsledná hodnota bonusu po úpravách
        /// </summary>
        public int FinalBonus => StatModifierTable.GetModifier(FinalValue) + BonusAdjustment;


        ///<example>
        /// RawValue = 15
        /// RawBonus = StatModifierTable.GetModifier(15) = 2
        
        /// ValueAdjustment = 2
        /// BonusAdjustment = 1
        
        /// FinlalValue = 15 + 2 = 17
        /// FinalBonus = StatModifierTable.GetModifier(17) + 1 = 3 + 1 = 4
        ///</example> 
        
        /// <SattModifierTable>
        /// 15-16 = +2
        /// 17-18 = +3
        /// 19-20 = +4
        /// </SattModifierTable>

    }

}
