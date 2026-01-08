using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;

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
            //currencyGroup.SetId(1);
            return currencyGroup;
        }

        public static Treasure ArrangeTreasure()
        {
            CurrencyGroup currencyGroup = ArrangeCurrencGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1,50)
                .AddCoins(2,75)
                .AddCoins(3,10);
            return treasure;
        }
    }
}
