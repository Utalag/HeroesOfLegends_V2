using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;
using Xunit;

namespace HoL.DomainTest.ObjectValue
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="Treasure"/>.
    /// Ověřuje vytváření pokladu, manipulaci s mincemi, validaci a výpočty hodnot.
    /// </summary>
    public class TreasureTest
    {
        /// <summary>
        /// Arrange: Vytvoří testovací měnový systém s několika úrovněmi mincí.
        /// </summary>
        /// <returns></returns>
        private CurrencyGroup CreateTestCurrencyGroup()
        {
            var gold = new SingleCurrency("Gold", "gl", 1, 1);
            var silver = new SingleCurrency("Silver", "sl", 2, 10);
            var copper = new SingleCurrency("Copper", "cp", 3, 100);
            return new CurrencyGroup("Fantasy Coins", new List<SingleCurrency> { gold, silver, copper });
        }

        /// <summary>
        /// Ověří, že nová instance <see cref="Treasure"/> je vytvořena s prázdným pokladem inicializovaným podle měnového systému.
        /// </summary>
        [Fact]
        public void Test_Treasure_Creation()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();

            // Act
            var treasure = new Treasure(currencyGroup);

            // Assert
            Assert.NotNull(treasure);
            Assert.Equal(currencyGroup, treasure.CurrencyGroup);
            Assert.Equal(currencyGroup.Id, treasure.CurrencyGroupId);
            Assert.Equal(3, treasure.CoinQuantities.Count);
            Assert.All(treasure.CoinQuantities.Values, quantity => Assert.Equal(0, quantity));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí <see cref="ArgumentNullException"/> při pokusu vytvořit poklad s null měnovým systémem.
        /// </summary>
        [Fact]
        public void Test_Treasure_Creation_NullCurrencyGroup_ThrowsException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Treasure(null));
            Assert.Equal("currencyGroup", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že konstruktor s coinQuantities správně inicializuje poklad s danými hodnotami.
        /// </summary>
        [Fact]
        public void Test_Treasure_CreationWithCoinQuantities()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var initialCoins = new Dictionary<int, int>
            {
                { 1, 50 },
                { 2, 100 },
                { 3, 200 }
            };

            // Act
            var treasure = new Treasure(currencyGroup, initialCoins);

            // Assert
            Assert.Equal(50, treasure.CoinQuantities[1]);
            Assert.Equal(100, treasure.CoinQuantities[2]);
            Assert.Equal(200, treasure.CoinQuantities[3]);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.AddCoins"/> správně přidá mince na daný level.
        /// </summary>
        [Fact]
        public void Test_Treasure_AddCoins()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act
            treasure.AddCoins(1, 50);
            treasure.AddCoins(2, 100);

            // Assert
            Assert.Equal(50, treasure.CoinQuantities[1]);
            Assert.Equal(100, treasure.CoinQuantities[2]);
            Assert.Equal(0, treasure.CoinQuantities[3]);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.AddCoins"/> podporuje fluent API.
        /// </summary>
        [Fact]
        public void Test_Treasure_AddCoins_FluentAPI()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();

            // Act
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 50)
                .AddCoins(2, 100)
                .AddCoins(3, 200);

            // Assert
            Assert.Equal(50, treasure.CoinQuantities[1]);
            Assert.Equal(100, treasure.CoinQuantities[2]);
            Assert.Equal(200, treasure.CoinQuantities[3]);
        }

        /// <summary>
        /// Ověří, že přidání mincí s neexistujícím levelem vyhodí <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void Test_Treasure_AddCoins_InvalidLevel_ThrowsException()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => treasure.AddCoins(99, 10));
            Assert.Equal("hierarchyLevel", exception.ParamName);
            Assert.Contains("neexistuje", exception.Message);
        }

        /// <summary>
        /// Ověří, že přidání záporného množství mincí vyhodí <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Fact]
        public void Test_Treasure_AddCoins_NegativeAmount_ThrowsException()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => treasure.AddCoins(1, -10));
            Assert.Equal("amount", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.RemoveCoins"/> správně odebere mince z daného levelu.
        /// </summary>
        [Fact]
        public void Test_Treasure_RemoveCoins()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 100);

            // Act
            treasure.RemoveCoins(1, 30);

            // Assert
            Assert.Equal(70, treasure.CoinQuantities[1]);
        }

        /// <summary>
        /// Ověří, že pokus o odebrání více mincí než je k dispozici vyhodí <see cref="InvalidOperationException"/>.
        /// </summary>
        [Fact]
        public void Test_Treasure_RemoveCoins_InsufficientFunds_ThrowsException()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 50);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => treasure.RemoveCoins(1, 100));
            Assert.Contains("není dostatek", exception.Message);
        }

        /// <summary>
        /// Ověří, že odebrání mincí s neexistujícím levelem vyhodí <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void Test_Treasure_RemoveCoins_InvalidLevel_ThrowsException()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => treasure.RemoveCoins(99, 10));
            Assert.Equal("hierarchyLevel", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.ClearCoins"/> odstraní všechny mince z pokladu.
        /// </summary>
        [Fact]
        public void Test_Treasure_ClearCoins()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 100)
                .AddCoins(2, 200)
                .AddCoins(3, 300);

            // Act
            treasure.ResetCoins();

            // Assert
            Assert.All(treasure.CoinQuantities.Values, quantity => Assert.Equal(0, quantity));
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.SetCoinQuantities"/> správně nastaví množství mincí.
        /// </summary>
        [Fact]
        public void Test_Treasure_SetCoinQuantities()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);
            var newQuantities = new Dictionary<int, int>
            {
                { 1, 75 },
                { 2, 150 }
            };

            // Act
            treasure.SetCoinQuantities(newQuantities);

            // Assert
            Assert.Equal(75, treasure.CoinQuantities[1]);
            Assert.Equal(150, treasure.CoinQuantities[2]);
            Assert.Equal(0, treasure.CoinQuantities[3]);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.SetCoinQuantities"/> ignoruje neexistující levely.
        /// </summary>
        [Fact]
        public void Test_Treasure_SetCoinQuantities_InvalidLevel_Ignored()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);
            var newQuantities = new Dictionary<int, int>
            {
                { 1, 50 },
                { 99, 100 } // Neexistující level
            };

            // Act
            treasure.SetCoinQuantities(newQuantities);

            // Assert
            Assert.Equal(50, treasure.CoinQuantities[1]);
            Assert.False(treasure.CoinQuantities.ContainsKey(99));
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.SetCurrencyGroup"/> změní měnový systém a reinicializuje poklad.
        /// </summary>
        [Fact]
        public void Test_Treasure_SetCurrencyGroup()
        {
            // Arrange
            var currencyGroup1 = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup1)
                .AddCoins(1, 100);

            var gold2 = new SingleCurrency("Euro", "eu", 1, 1);
            var currencyGroup2 = new CurrencyGroup("Modern Money", new List<SingleCurrency> { gold2 });

            // Act
            treasure.SetCurrencyGroup(currencyGroup2);

            // Assert
            Assert.Equal(currencyGroup2, treasure.CurrencyGroup);
            Assert.Equal(currencyGroup2.Id, treasure.CurrencyGroupId);
            Assert.Single(treasure.CoinQuantities);
            Assert.Equal(0, treasure.CoinQuantities[1]); // Reinicializováno na 0
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.SetCurrencyGroup"/> vyhodí výjimku při null parametru.
        /// </summary>
        [Fact]
        public void Test_Treasure_SetCurrencyGroup_Null_ThrowsException()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => treasure.SetCurrencyGroup(null));
            Assert.Equal("currencyGroup", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.GetTotalValueInBaseUnits"/> správně vypočítá celkovou hodnotu.
        /// </summary>
        [Fact]
        public void Test_Treasure_GetTotalValueInBaseUnits()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 5)    // 5 * 1 = 5
                .AddCoins(2, 3)    // 3 * 10 = 30
                .AddCoins(3, 2);   // 2 * 100 = 200

            // Act
            var totalValue = treasure.GetTotalValueInBaseUnits();

            // Assert
            Assert.Equal(235, totalValue); // 5 + 30 + 200
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.GetTotalValueInBaseUnits"/> vrací 0 pro prázdný poklad.
        /// </summary>
        [Fact]
        public void Test_Treasure_GetTotalValueInBaseUnits_Empty()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act
            var totalValue = treasure.GetTotalValueInBaseUnits();

            // Assert
            Assert.Equal(0, totalValue);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.ToString"/> vrací správný textový popis pokladu.
        /// </summary>
        [Fact]
        public void Test_Treasure_ToString()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup)
                .AddCoins(1, 50)
                .AddCoins(2, 100);

            // Act
            var result = treasure.ToString();

            // Assert
            Assert.Contains("50", result);
            Assert.Contains("100", result);
            Assert.Contains("Gold", result);
            Assert.Contains("Silver", result);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="Treasure.ToString"/> vrací "Prázdný poklad" pro prázdný poklad.
        /// </summary>
        [Fact]
        public void Test_Treasure_ToString_Empty()
        {
            // Arrange
            var currencyGroup = CreateTestCurrencyGroup();
            var treasure = new Treasure(currencyGroup);

            // Act
            var result = treasure.ToString();

            // Assert
            Assert.Equal("Prázdný poklad", result);
        }
    }
}
