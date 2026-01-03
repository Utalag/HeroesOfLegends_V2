using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects;

namespace HoL.Domain.Builders
{
    public class TreasureBuilder
    {
        private int _id;
        private CurrencyGroup? _currencyGroup;
        private Dictionary<int, int> _coinQuantities = new();
        private Treasure? _existingTreasure;

        #region Update existující instance

        /// <summary>
        /// Nastaví builder pro úpravu existující instance Treasure.
        /// </summary>
        public TreasureBuilder FromExisting(Treasure treasure)
        {
            if (treasure == null)
                throw new ArgumentNullException(nameof(treasure));

            _id = treasure.Id;
            _currencyGroup = treasure.CurrencyGroup;
            _coinQuantities = new Dictionary<int, int>(treasure.CoinQuantities);
            _existingTreasure = treasure;
            return this;
        }

        #endregion

        #region Nastavení parametrů

        /// <summary>
        /// Nastaví ID pokladu.
        /// </summary>
        public TreasureBuilder WithId(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id nemůže být menší než 0");

            _id = id;
            return this;
        }

        /// <summary>
        /// Nastaví měnový systém pomocí builderu.
        /// </summary>
        public TreasureBuilder WithCurrencyGroup(CurrencyGroupBuilder currencyGroupBuilder)
        {
            if (currencyGroupBuilder == null)
                throw new ArgumentNullException(nameof(currencyGroupBuilder));

            _currencyGroup = currencyGroupBuilder.Build();
            return this;
        }

        /// <summary>
        /// Nastaví měnový systém přímo.
        /// </summary>
        public TreasureBuilder WithCurrencyGroup(CurrencyGroup currencyGroup)
        {
            if (currencyGroup == null)
                throw new ArgumentNullException(nameof(currencyGroup));

            _currencyGroup = currencyGroup;
            return this;
        }

        #endregion

        #region Operace s mincemi

        /// <summary>
        /// Nastaví množství mincí pro daný level.
        /// </summary>
        /// <param name="level">Level měny</param>
        /// <param name="amount">Množství mincí (0 nebo více)</param>
        /// <returns>Builder instance pro fluent API</returns>
        /// <exception cref="ArgumentOutOfRangeException">Pokud je množství záporné</exception>
        public TreasureBuilder WithCoinQuantity(int level, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Množství nemůže být záporné");

            _coinQuantities[level] = amount;
            return this;
        }

        /// <summary>
        /// Přidá množství mincí k existujícímu množství na daném levelu.
        /// </summary>
        /// <param name="level">Level měny</param>
        /// <param name="amount">Množství mincí k přidání</param>
        /// <returns>Builder instance pro fluent API</returns>
        /// <exception cref="ArgumentOutOfRangeException">Pokud je množství záporné</exception>
        public TreasureBuilder AddCoinQuantityAtLevel(int level, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Množství nemůže být záporné");

            if (!_coinQuantities.ContainsKey(level))
                _coinQuantities[level] = 0;

            _coinQuantities[level] += amount;
            return this;
        }

        /// <summary>
        /// Vymaže množství mincí na daném levelu (nastaví na 0).
        /// </summary>
        /// <param name="level">Level měny</param>
        /// <returns>Builder instance pro fluent API</returns>
        public TreasureBuilder ClearCoinQuantityAtLevel(int level)
        {
            _coinQuantities[level] = 0;
            return this;
        }

        /// <summary>
        /// Vymaže všechny množství mincí.
        /// </summary>
        public TreasureBuilder ClearAllCoinQuantities()
        {
            _coinQuantities.Clear();
            return this;
        }

        #endregion

        /// <summary>
        /// Vytvoří nebo aktualizuje Treasure.
        /// </summary>
        public Treasure Build()
        {
            if (_currencyGroup == null)
                throw new InvalidOperationException("CurrencyGroup je povinný");

            if (_existingTreasure != null)
            {
                // UPDATE - vytvoříme novou instanci s aktualizovanými daty
                _existingTreasure.CurrencyGroup = _currencyGroup;
                _existingTreasure.Id = _id;
                foreach (var kvp in _coinQuantities)
                {
                    _existingTreasure.CoinQuantities.Add(kvp.Key, kvp.Value);
                }
                _existingTreasure.CurrencyGroupId = _currencyGroup.Id;

                return _existingTreasure;

                //return new Treasure(_id > 0 ? _id : _existingTreasure.Id, _currencyGroup, _coinQuantities);
            }
            else
            {
                // CREATE
                if (_id > 0)
                {
                    // S ID (z databáze) - použijeme internal konstruktor
                    return new Treasure(_id, _currencyGroup, _coinQuantities);
                }
                else
                {
                    // Nová entita bez ID - použijeme veřejný konstruktor
                    var treasure = new Treasure(_currencyGroup);
                    foreach (var kvp in _coinQuantities)
                    {
                        treasure.CoinQuantities[kvp.Key] = kvp.Value;
                    }
                    return treasure;
                }
            }
        }
    }
}
