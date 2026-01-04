using System.Diagnostics;

namespace HoL.Domain.ValueObjects.CurrencyObjects
{
    /// <summary>
    /// Reprezentuje jednu hodnotu měny.
    /// </summary>
    [DebuggerDisplay("{ShotName}")]
    public class SingleCurrency
    {
        public int Id { get; private set; }

        /// <summary>
        /// Název měny.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Krátký název měny.
        /// </summary>
        public string ShotName { get; private set; } = string.Empty;

        /// <summary>
        /// Úroveň (1 = nejvyšší hodnat, vyšší číslo = nižší hodnota).
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Směnný kurz vůči základní denominaci.
        /// </summary>
        public int ExchangeRate { get; private set; } = 1;

        /// <summary>Konstruktor pro vytvoření jedné měnové jednotky.</summary>
        /// <param name="name">Plný název měny</param>
        /// <param name="shortName">Zkratka měny</param>
        /// <param name="level">Úroveň (1 = nejnižší denominace, vyšší = cennější)</param>
        /// <param name="exchangeRate">Směnný kurz vůči nižšímu levelu</param>
        /// <exception cref="ArgumentException">Vyvoláno pokud je 'name' nebo 'shortName' prázdné</exception>
        /// <exception cref="ArgumentOutOfRangeException">Vyvoláno pokud je 'level' nebo 'exchangeRate' menší než 1</exception>
        /// <example>
        /// <code>
        /// var galeon = new SingleCurrency("Galeon", "gl", 1, 1);
        /// var sickle = new SingleCurrency("Sickel", "sl", 2, 17);
        /// var knut = new SingleCurrency("Knut", "kn", 3, 29);
        /// </code>
        /// </example>
        public SingleCurrency(string name, string shortName, int level, int exchangeRate)
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
        /// Nastaví identifikátor. Používá se jen při načítání/persistenci.
        /// </summary>
        /// <param name="id">Nezáporné ID, které přiřadí ORM/builder.</param>
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID nesmí být záporné");

            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}, {Level}, {ShotName}, {Name}, {ExchangeRate}";
        }
    }
}
