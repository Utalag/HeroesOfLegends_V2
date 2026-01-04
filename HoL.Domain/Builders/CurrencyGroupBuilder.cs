using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects.CurrencyObjects;

namespace HoL.Domain.Builders
{
    public class CurrencyGroupBuilder
    {
        private int _id;
        private string _groupName = string.Empty;
        private List<SingleCurrency> _currencyList = new();
        private CurrencyGroup? _existingCurrencyGroup;

        #region Update existující instance

        /// <summary>
        /// Nastaví builder pro úpravu existující skupiny měn.
        /// </summary>
        public CurrencyGroupBuilder FromExisting(CurrencyGroup currencyGroup)
        {
            if (currencyGroup == null)
                throw new ArgumentNullException(nameof(currencyGroup));

            _existingCurrencyGroup = currencyGroup;
            _id = currencyGroup.Id;
            _groupName = currencyGroup.GroupName;
            _currencyList = currencyGroup.Currencies.ToList();
            return this;
        }

        #endregion

        #region Nastavení parametrů

        public CurrencyGroupBuilder WithId(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id nemůže být záporné");

            _id = id;
            return this;
        }

        public CurrencyGroupBuilder WithGroupName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Název skupiny nemůže být prázdný", nameof(groupName));

            _groupName = groupName;
            return this;
        }

        public CurrencyGroupBuilder WithCurrencies(List<SingleCurrency> currencies)
        {
            if (currencies == null || currencies.Count == 0)
                throw new ArgumentException("Seznam měn nemůže být prázdný", nameof(currencies));

            _currencyList = currencies;
            return this;
        }

        #endregion

        #region Operace s měnami

        /// <summary>
        /// Přidá měnu do builderu s validací duplicity.
        /// </summary>
        public CurrencyGroupBuilder AddCurrency(SingleCurrency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency), "Měna nemůže být null");

            if (_currencyList.Any(c => c.Level == currency.Level))
            {
                throw new InvalidOperationException(
                    $"Měna s úrovní {currency.Level} již existuje");
            }

            _currencyList.Add(currency);
            return this;
        }

        public CurrencyGroupBuilder ClearCurrencies()
        {
            _currencyList.Clear();
            return this;
        }

        public CurrencyGroupBuilder RemoveCurrency(SingleCurrency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency), "Měna nemůže být null");

            _currencyList.Remove(currency);
            return this;
        }

        public CurrencyGroupBuilder RemoveCurrencyByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Název měny nemůže být prázdný", nameof(name));

            var currencyToRemove = _currencyList.FirstOrDefault(c => c.Name == name);
            if (currencyToRemove != null)
            {
                _currencyList.Remove(currencyToRemove);
            }
            return this;
        }

        public CurrencyGroupBuilder RemoveCurrencyByLevel(int level)
        {
            if (level <= 0)
                throw new ArgumentOutOfRangeException(nameof(level), "Úroveň měny musí být kladná");

            var currencyToRemove = _currencyList.FirstOrDefault(c => c.Level == level);
            if (currencyToRemove != null)
            {
                _currencyList.Remove(currencyToRemove);
            }
            return this;
        }

        #endregion

        /// <summary>
        /// Vytvoří nebo aktualizuje CurrencyGroup.
        /// </summary>
        public CurrencyGroup Build()
        {
            if (string.IsNullOrWhiteSpace(_groupName))
                throw new InvalidOperationException("Název skupiny měn je povinný");

            if (_existingCurrencyGroup != null)
            {
                // UPDATE - modifikujeme existující instanci
                _existingCurrencyGroup.GroupName = _groupName;
                _existingCurrencyGroup._currencies.Clear();
                _existingCurrencyGroup._currencies.AddRange(_currencyList);
                return _existingCurrencyGroup;
            }
            else
            {
                // CREATE - vytvoříme novou instanci
                var currencyGroup = new CurrencyGroup(_groupName);
                if (_id > 0)
                {
                    currencyGroup.Id = _id;
                }

                foreach (var currency in _currencyList)
                {
                    currencyGroup.AddCurrency(currency);
                }
                return currencyGroup;
            }
        }
    }
}