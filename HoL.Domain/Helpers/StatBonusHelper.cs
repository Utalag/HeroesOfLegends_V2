namespace HoL.Domain.Helpers
{
    public static class StatBonusHelper
    {
        public static int GetBonus(int stat)
        {
            return stat switch
            {
                <= 1 => -5,
                <= 3 => -4,
                <= 5 => -3,
                <= 7 => -2,
                <= 9 => -1,
                <= 12 => 0,
                <= 14 => +1,
                <= 16 => +2,
                <= 18 => +3,
                <= 20 => +4,
                <= 22 => +5,
                <= 24 => +6,
                <= 26 => +7,
                <= 28 => +8,
                <= 30 => +9,
                <= 32 => +10,
                <= 34 => +11,
                <= 36 => +12,
                <= 38 => +13,
                <= 40 => +14,
                <= 42 => +15,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

    }
}
