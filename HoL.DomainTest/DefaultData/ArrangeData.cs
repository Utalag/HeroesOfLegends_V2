using HoL.Domain.Entities.CurencyEntities;

namespace HoL.DomainTest.DefaultData
{
    public static class ArrangeData
    {
        public static CurrencyGroup ArrangeCurrencGroup()
        {
            var arrangeGold = new SingleCurrency("Gold", "zl", 1, 1);
            var arrangeSilver = new SingleCurrency("Silver", "st", 2, 10);
            var arrangeCopper = new SingleCurrency("Copper", "md", 3, 100);

            var currencyGroup = new CurrencyGroup("FantasyCoins", new() {arrangeGold,arrangeSilver,arrangeCopper });

            return currencyGroup;
        }

        //public static Treasure ArrangeTreasure()
        //{
        //    CurrencyGroup currencyGroup = ArrangeCurrencGroup();
        //    var treasure = new Treasure(currencyGroup);
        //    treasure.Add(1, 50);  // 50 Gold
        //    treasure.Add(2, 200); // 200 Silver
        //    treasure.Add(3, 500); // 500 Copper
        //    return treasure;
        //}
    }
}
