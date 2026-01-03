using System.Diagnostics;

namespace HoL.Domain.Helpers
{
    /// <summary>
    /// Reprezentuje jednu denominaci měny (např. měděná mince, stříbrná mince).
    /// </summary>
    [DebuggerDisplay("{ShotName}")]
    public class SingleCurrency
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ShotName { get; private set; }
        public int Level { get; private set; }
        public int ExchangeRate { get; private set; }

        /// <summary>
        /// Privátní konstruktor pro EF Core.
        /// </summary>
        private SingleCurrency()
        {
            Name = string.Empty;
            ShotName = string.Empty;
            ExchangeRate = 1;
        }

        /// <summary>
        /// Vytvoří novou měnu s validací všech polí.
        /// </summary>
        /// <param name="name">Plný název měny</param>
        /// <param name="shortName">Zkratka měny</param>
        /// <param name="level">Úroveň (1 = nejnižší, vyšší = vyšší denominace)</param>
        /// <param name="exchangeRate">Směnný kurz vůči základní měně</param>
        public SingleCurrency(string name, string shortName, int level, int exchangeRate) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Název měny nesmí být prázdný", nameof(name));

            if (string.IsNullOrWhiteSpace(shortName))
                throw new ArgumentException("Zkratka měny nesmí být prázdná", nameof(shortName));

            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level), "Úroveň musí být kladné číslo");

            if (exchangeRate < 1)
                throw new ArgumentOutOfRangeException(nameof(exchangeRate), "Směnný kurz musí být kladné číslo");

            Name = name;
            ShotName = shortName;
            Level = level;
            ExchangeRate = exchangeRate;
        }

        /// <summary>
        /// Interní metoda pro nastavení ID (používá se Repository a Builder).
        /// </summary>
        internal void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}, {Level}, {ShotName}, {Name}, {ExchangeRate}";
        }
    }
}
