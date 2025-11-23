using HoL.Domain.Enums;

namespace HoL.Domain.Interfaces.Write_Interaces
{
    public interface IStatWrite
    {
        StatType Type { get; set; }
        int RawValue { get; set; }
        int ValueAdjustment { get; set; }
        int BonusAdjustment { get; set; }
    }
}
