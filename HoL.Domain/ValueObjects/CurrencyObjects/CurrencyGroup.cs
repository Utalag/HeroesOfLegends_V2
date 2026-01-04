using System.Diagnostics;
using HoL.Domain.ValueObjects.CurrencyObjects;

namespace HoL.Domain.Helpers
{
    /// <summary>
    /// Agregát reprezentující skupinu měn se všemi denominacemi.
    /// </summary>
    [DebuggerDisplay("{GroupName}")]
    public class CurrencyGroup
    {
        public int Id { get; internal set; }
        public string GroupName { get; internal set; } = string.Empty;
        public IReadOnlyList<SingleCurrency> Currencies => _currencies.AsReadOnly();

        internal readonly List<SingleCurrency> _currencies = new();


        public CurrencyGroup(string groupName) 
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Název měnového systému nesmí být prázdný", nameof(groupName));

            GroupName = groupName;
        }

        /// <summary>
        /// Přidá měnu s validací business pravidel.
        /// </summary>
        internal void AddCurrency(SingleCurrency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency), "Měna nemůže být null");

            if (_currencies.Any(c => c.Level == currency.Level))
            {
                throw new InvalidOperationException(
                    $"Měna s úrovní {currency.Level} již existuje");
            }

            _currencies.Add(currency);
        }

        public SingleCurrency? GetByLevel(int level)
        {
            return _currencies.FirstOrDefault(c => c.Level == level);
        }
    }
}
