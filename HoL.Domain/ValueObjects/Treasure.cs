using System.Security.Cryptography;
using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects;

namespace HoL.Domain.ValueObjects
{
    /// <summary>
    /// Value object reprezentující poklad entity s různými denominacemi měn.
    /// </summary>
    /// <remarks>
    /// CoinQuantities se inicializuje podle CurrencyGroup.Currencies.
    /// Každý Currencies.Level vytvoří klíč ve slovníku.
    /// </remarks>
    public class Treasure
    {
        public int Id { get; internal set; }
        public Dictionary<int, int> CoinQuantities { get; internal set; } = new Dictionary<int, int>();
        public int CurrencyGroupId { get; internal set; }
        public CurrencyGroup CurrencyGroup { get; internal set; }

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
        internal Treasure(int id, CurrencyGroup currencyGroup, Dictionary<int, int> coinQuantities) : this(currencyGroup)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id nemůže být záporné");

            Id = id;
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
            foreach (var currency in CurrencyGroup.Currencies.OrderBy(x => x.Level))
            {
                CoinQuantities[currency.Level] = 0;
            }
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
                var currency = CurrencyGroup.Currencies.FirstOrDefault(t => t.Level == kvp.Key);
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
                var currency = CurrencyGroup.Currencies.FirstOrDefault(t => t.Level == kvp.Key);
                var coinName = currency?.Name ?? $"Level {kvp.Key}";
                parts.Add($"{kvp.Value} {coinName}");
            }

            return parts.Any() ? string.Join(", ", parts) : "Prázdný poklad";
        }
    }
}
