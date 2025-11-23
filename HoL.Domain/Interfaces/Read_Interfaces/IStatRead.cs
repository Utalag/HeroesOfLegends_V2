using HoL.Domain.Enums;

namespace HoL.Domain.Interfaces.Read_Interfaces
{
    public interface IStatRead
    {
        StatType Type { get; }
        public int FinalValue { get; }
        public int FinalBonus { get; }
        int RawBonus { get; }
        int RawValue { get; set; }
    }
}
