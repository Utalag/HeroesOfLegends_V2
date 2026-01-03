using HoL.Domain.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace HoL.DomainTest.TestModels
{
    public class CurrencyGroupTest
    {


        [Fact]
        public void Test_CurrencyGroup_Creation()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Gold Standard");
            // Assert
            Assert.NotNull(currencyGroup);
            Assert.Equal("Gold Standard", currencyGroup.GroupName);
            Assert.Empty(currencyGroup.Currencies);
        }

        [Fact]
        public void Test_CurrencyGroup_Add_SingleCurrency()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            // Act
            currencyGroup.Add(gold);
            // Assert
            Assert.Single(currencyGroup.Currencies);
            Assert.Contains(gold, currencyGroup.Currencies);
        }

        [Fact]
        public void Test_CurrencyGroup_Add_MultipleCurrencies()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Medieval CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            var silver = new SingleCurrency("Silver", "st", 2, 10);
            var copper = new SingleCurrency("Copper", "md", 3, 100);
            // Act
            currencyGroup.Add(gold);
            currencyGroup.Add(silver);
            currencyGroup.Add(copper);
            // Assert
            Assert.Equal(3, currencyGroup.Currencies.Count);

        }

        [Fact]
        public void Test_CurrencyGroup_Add_DuplicateLevel_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold1 = new SingleCurrency("Gold", "zl", 1, 1);
            var gold2 = new SingleCurrency("Another Gold", "ag", 1, 5);  // Same Level!
            currencyGroup.Add(gold1);
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => currencyGroup.Add(gold2));
            Assert.Contains("úrovní", exception.Message, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Test_CurrencyGroup_Add_NullCurrency_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => currencyGroup.Add(null));
            Assert.Equal("currencyGroup", exception.ParamName);
        }

        [Fact]
        public void Test_CurrencyGroup_Remove()
        {
            CurrencyGroup currencyGroup;
            SingleCurrency gold;
            SingleCurrency silver;

            // Arrange
            currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            gold = new SingleCurrency("Gold", "zl", 1, 1);
            silver = new SingleCurrency("Silver", "st", 2, 10);
            currencyGroup.Add(gold);
            currencyGroup.Add(silver);
            // Act
            var result = currencyGroup.Remove(gold);
            // Assert
            Assert.True(result);
            Assert.Single(currencyGroup.Currencies);
            Assert.DoesNotContain(gold, currencyGroup.Currencies);
        }

        [Fact]
        public void Test_CurrencyGroup_RemoveByName()
        {

            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            var silver = new SingleCurrency("Silver", "st", 2, 10);

            currencyGroup.Add(gold);
            currencyGroup.Add(silver);

            // Act
            currencyGroup.RemoveByName("Silver");
            // Assert
            Assert.Single(currencyGroup.Currencies);

            Assert.Null(currencyGroup.GetByName("Silver"));
        }

        [Fact]
        public void Test_CurrencyGroup_RemoveByName_NotFound_ThrowsException()
        {

            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => currencyGroup.RemoveByName("NonExistent"));

            Assert.Contains("neexistuje", exception.Message, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Test_CurrencyGroup_RemoveById()
        {

            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            var silver = new SingleCurrency("Silver", "st", 2, 10);

            currencyGroup.Add(gold);
            currencyGroup.Add(silver);

            // Act
            var result = currencyGroup.RemoveById(gold.Id);

            // Assert
            Assert.True(result);
            Assert.Single(currencyGroup.Currencies);
        }

        [Fact]
        public void Test_CurrencyGroup_Update()
        {

            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            currencyGroup.Add(gold);
            // Act
            var updatedGold = new SingleCurrency("Gold", "zl", 1, 10);
            currencyGroup.Update(updatedGold);
            // Assert
            var goldFromGroup = currencyGroup.GetByName("Gold");
            Assert.NotNull(goldFromGroup);
            Assert.Equal(10, goldFromGroup.ExchangeRate);
            Assert.Equal(gold.Id, goldFromGroup.Id);
        }

        [Fact]
        public void Test_CurrencyGroup_Clear()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            currencyGroup.Add(new SingleCurrency("Gold", "zl", 1, 1));
            currencyGroup.Add(new SingleCurrency("Silver", "st", 2, 10));
            currencyGroup.Add(new SingleCurrency("Copper", "md", 3, 100));
            // Act
            currencyGroup.ClearCurrencies();
            // Assert
            Assert.Empty(currencyGroup.Currencies);
        }

        [Fact]
        public void Test_CurrencyGroup_RenameGlobalName()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Old Name");
            // Act
            currencyGroup.SetGroupName("New Name");
            // Assert
            Assert.Equal("New Name", currencyGroup.GroupName);
        }

        [Fact]
        public void Test_CurrencyGroup_GetByName()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            currencyGroup.Add(gold);
            // Act
            var retrieved = currencyGroup.GetByName("Gold");
            // Assert
            Assert.NotNull(retrieved);
            Assert.Equal("Gold", retrieved.Name);
        }

        [Fact]
        public void Test_CurrencyGroup_GetByLevel()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            currencyGroup.Add(new SingleCurrency("Gold", "zl", 1, 1));
            currencyGroup.Add(new SingleCurrency("Silver", "st", 2, 10));
            // Act
            var silver = currencyGroup.GetByLevel(2);
            // Assert
            Assert.NotNull(silver);
            Assert.Equal("Silver", silver.Name);
        }

        [Fact]
        public void Test_SingleCurrency_Creation()
        {
            // Arrange & Act
            var currency = new SingleCurrency("Gold", "zl", 1, 100);
            // Assert
            Assert.Equal("Gold", currency.Name);
            Assert.Equal("zl", currency.ShotName);
            Assert.Equal(1, currency.Level);
            Assert.Equal(100, currency.ExchangeRate);
        }

        [Fact]
        public void Test_SingleCurrency_InvalidLevel_ThrowsException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SingleCurrency("Gold", "zl", 0, 100)
            );
            Assert.Equal("level", exception.ParamName);
        }

        [Fact]
        public void Test_SingleCurrency_InvalidExchangeRate_ThrowsException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SingleCurrency("Gold", "zl", 1, 0)
            );
            Assert.Equal("exchangeRate", exception.ParamName);
        }

        [Fact]
        public void Test_SingleCurrency_EmptyName_ThrowsException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new SingleCurrency("", "zl", 1, 100)
            );
            Assert.Equal("name", exception.ParamName);
        }
    }
}
