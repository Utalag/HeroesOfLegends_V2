using System.Diagnostics;

namespace HoL.Domain.Entities.CurencyEntities
{
    /// <summary>
    /// Agregát reprezentující skupinu měn se všemi denominacemi.
    /// </summary>
    [DebuggerDisplay("{GroupName}")]
    public class CurrencyGroup
    {
        /// <summary>
        /// Unikátní identifikátor skupiny měn.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Název skupiny měn.
        /// </summary>
        public string GroupName { get; internal set; } = string.Empty;

        /// <summary>
        /// Kolekce jednotlivých měnových denominací.
        /// </summary>
        public IReadOnlyList<SingleCurrency> Currencies => _currencies.AsReadOnly();

        internal readonly List<SingleCurrency> _currencies = new();


        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="CurrencyGroup"/> s názvem a počáteční sadou měn.
        /// </summary>
        /// <param name="groupName">Název měnové skupiny</param>
        /// <param name="singleCurrencies">Seznam měn pro inicializaci</param>
        public CurrencyGroup(string groupName, List<SingleCurrency> singleCurrencies)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Název měnového systému nesmí být prázdný", nameof(groupName));
            GroupName = groupName;

            _currencies.Clear();

            if (singleCurrencies == null)
                throw new ArgumentNullException(nameof(singleCurrencies), "Seznam měn nemůže být null");

            foreach (var currency in singleCurrencies)
            {
                AddCurrency(currency);
            }

        }

        /// <summary>
        /// Přidá měnu do skupiny s validací unikátnosti úrovně.
        /// </summary>
        /// <param name="currency">Měna k přidání</param>
        /// <exception cref="ArgumentNullException">Vyvoláno pokud je měna null</exception>
        /// <exception cref="InvalidOperationException">Vyvoláno pokud měna s danou úrovní již existuje</exception>
        public void AddCurrency(SingleCurrency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency), "Měna nemůže být null");

            if (_currencies.Any(c => c.HierarchyLevel == currency.HierarchyLevel))
            {
                throw new InvalidOperationException(
                    $"Měna s úrovní {currency.HierarchyLevel} již existuje");
            }

            _currencies.Add(currency);
        }

        /// <summary>
        /// Odstraní měnu ze skupiny.
        /// </summary>
        /// <param name="currency">Měna k odebrání</param>
        /// <returns>true pokud byla měna odstraněna; false pokud se měna ve skupině nenacházela</returns>
        /// <exception cref="ArgumentNullException">Vyvoláno pokud je měna null</exception>
        public bool RemoveCurrency(SingleCurrency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency), "Měna nemůže být null");

            return _currencies.Remove(currency);
        }

        /// <summary>
        /// Vrátí měnu podle její úrovně.
        /// </summary>
        /// <param name="level">Úroveň hledané měny</param>
        /// <returns>Měna s danou úrovní nebo null pokud neexistuje</returns>
        public SingleCurrency? GetByLevel(int level)
        {
            return _currencies.FirstOrDefault(c => c.HierarchyLevel == level);
        }

        /// <summary>
        /// Změní název skupiny měn.
        /// </summary>
        /// <param name="newName">Nový název skupiny</param>
        /// <exception cref="ArgumentException">Vyvoláno pokud je název prázdný</exception>
        public void SetGroupName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Název měnového systému nesmí být prázdný", nameof(newName));

            GroupName = newName;
        }
    }
}
