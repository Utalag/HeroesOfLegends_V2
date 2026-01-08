using HoL.Domain.Entities.CurencyEntities;

namespace HoL.Domain.ValueObjects
{
    /// <summary>
    /// Value object reprezentující poklad entity s různými denominacemi měn.
    /// </summary>
    /// <remarks>
    /// CoinQuantities se inicializuje podle CurrencyGroup.Currencies.
    /// Každý Currencies.HierarchyLevel vytvoří klíč ve slovníku.
    /// </remarks>
    public class Treasure
    {

        //ToDo: unit testy


        /// <summary>
        /// Slovník mapující HierarchyLevel měny na množství mincí v dané denominaci.
        /// </summary>
        public Dictionary<int, int> CoinQuantities { get; private set; } = new Dictionary<int, int>();
        /// <summary>
        /// Identifikátor měnového systému.
        /// </summary>
        public int CurrencyGroupId { get; private set; }
        /// <summary>
        /// Instance měnového systému používaného v tomto pokladu.
        /// </summary>
        public CurrencyGroup CurrencyGroup { get; private set; }

        private Treasure()
        {
            CurrencyGroup = null!;
        }

        /// <summary>
        /// Vytvoří nový Treasure s danou měnou.
        /// </summary>
        /// <param name="currencyGroup">Měnový systém</param>
        public Treasure(CurrencyGroup currencyGroup) : this()
        {
            if (currencyGroup == null)
                throw new ArgumentNullException(nameof(currencyGroup));

            CurrencyGroup = currencyGroup;
            CurrencyGroupId = currencyGroup.Id;
            InitializeCoinQuantities();
        }

        /// <summary>
        /// Interní konstruktor pro builder - vytvoří poklad s kompletními daty.
        /// </summary>
        public Treasure(CurrencyGroup currencyGroup, Dictionary<int, int> coinQuantities) : this(currencyGroup)
        {

            CoinQuantities = new Dictionary<int, int>(coinQuantities);
        }
        
        /// <summary>
        /// Inicializuje CoinQuantities podle CurrencyGroup.Currencies.
        /// </summary>
        private void InitializeCoinQuantities()
        {
            if (CurrencyGroup?.Currencies == null)
                return;

            CoinQuantities.Clear();
            foreach (var currency in CurrencyGroup.Currencies)
            {
                CoinQuantities[currency.HierarchyLevel] = 0;
            }
        }

        /// <summary>
        /// Nastaví nový měnový system pro poklad a inicializuje CoinQuantities.
        /// </summary>
        /// <param name="currencyGroup"> <see cref="CurrencyGroup"/> nesmí být null</param>
        /// <returns> Aktuální <see cref="Treasure"/> instanci s upravenou <see cref="CurrencyGroup"/></returns>
        /// <exception cref="ArgumentNullException">Vyhozen pokud <paramref name="currencyGroup"/> je null.</exception>
        public Treasure SetCurrencyGroup(CurrencyGroup currencyGroup)
        {
            if (currencyGroup == null)
                throw new ArgumentNullException(nameof(currencyGroup));
            CurrencyGroup = currencyGroup;
            CurrencyGroupId = currencyGroup.Id;
            InitializeCoinQuantities();
            return this;
        }

        /// <summary>
        /// Nastaví nové množství mincí pro jednotlivé měnové levely.
        /// </summary>
        /// <param name="coinQuantities"></param>
        /// <returns></returns>
        public Treasure SetCoinQuantities(Dictionary<int, int> coinQuantities)
        {
            foreach (var item in coinQuantities)
            {
                if(CoinQuantities.ContainsKey(item.Key))
                {
                   CoinQuantities[item.Key] = item.Value;
                }
            }
            return this;
        }


        /// <summary>
        /// Přidání množství mincí na danému měnovému levelu.
        /// </summary>
        /// <param name="hierarchyLevel">Měnový level</param>
        /// <param name="amount">Množství mincí</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Treasure AddCoins(int hierarchyLevel, int amount)
        {
            if (!CoinQuantities.ContainsKey(hierarchyLevel))
                throw new ArgumentException($"HierarchyLevel {hierarchyLevel} neexistuje v měnovém systému.", nameof(hierarchyLevel));
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Množství mincí musí být 0 nebo více.");
            CoinQuantities[hierarchyLevel] += amount;
            return this;
        }

        /// <summary>
        ///  Odstranění množství mincí na danému měnovému levelu.
        /// </summary>
        /// <param name="hierarchyLevel">Měnový level</param>
        /// <param name="amount">Množství mincí</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Treasure RemoveCoins(int hierarchyLevel, int amount)
        {
            if (!CoinQuantities.ContainsKey(hierarchyLevel))
                throw new ArgumentException($"HierarchyLevel {hierarchyLevel} neexistuje v měnovém systému.", nameof(hierarchyLevel));
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Množství mincí musí být 0 nebo více.");
            if (CoinQuantities[hierarchyLevel] < amount)
                throw new InvalidOperationException($"Nelze odebrat {amount} mincí z HierarchyLevel {hierarchyLevel}, protože jich není dostatek.");
            CoinQuantities[hierarchyLevel] -= amount;
            return this;
        }

        /// <summary>
        /// Odstranění všech mincí z pokladu.
        /// </summary>
        /// <returns></returns>
        public Treasure ResetCoins()
        {
            foreach (var key in CoinQuantities.Keys.ToList())
            {
                CoinQuantities[key] = 0;
            }
            return this;
        }


        /// <summary>
        /// Vypočítá celkovou hodnotu v základních jednotkách (nejnižší denominace).
        /// </summary>
        public int GetTotalValueInBaseUnits()
        {
            if (CurrencyGroup?.Currencies == null)
                return 0;

            int total = 0;
            foreach (var kvp in CoinQuantities)
            {
                var currency = CurrencyGroup.Currencies.FirstOrDefault(t => t.HierarchyLevel == kvp.Key);
                if (currency != null)
                {
                    total += kvp.Value * currency.ExchangeRate;
                }
            }
            return total;
        }

        /// <summary>
        /// Textová reprezentace pokladu s názvy mincí z CurrencyGroup.
        /// </summary>
        public override string ToString()
        {
            if (CurrencyGroup?.Currencies == null || !CoinQuantities.Any(x => x.Value > 0))
                return "Prázdný poklad";

            var parts = new List<string>();
            foreach (var kvp in CoinQuantities.OrderByDescending(x => x.Key).Where(x => x.Value > 0))
            {
                var currency = CurrencyGroup.Currencies.FirstOrDefault(t => t.HierarchyLevel == kvp.Key);
                var coinName = currency?.Name ?? $"HierarchyLevel {kvp.Key}";
                parts.Add($"{kvp.Value} {coinName}");
            }

            return parts.Any() ? string.Join(", ", parts) : "Prázdný poklad";
        }
    }
}
