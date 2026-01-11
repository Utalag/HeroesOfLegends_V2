using HoL.Domain.Entities.CurencyEntities;
using Xunit;

namespace HoL.DomainTest.Entities
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="CurrencyGroup"/>.
    /// Ověřuje vytváření skupin měn, přidávání měn, jejich odebrání, přejmenování skupiny a validační pravidla.
    /// </summary>
    public class CurrencyGroupTest
    {
        // Default global Arrange
        string arrangeName = "Gold";
        SingleCurrency arrangeGold = new SingleCurrency("Gold", "gl", 1, 1);
        SingleCurrency arrangeSilver = new SingleCurrency("Silver", "sl", 2, 10);

        /// <summary>
        /// Ověří, že nová instance <see cref="CurrencyGroup"/> je vytvořena se správným názvem a nenulovou kolekcí.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Creation()
        {
            // Act
            var currencyGroup = new CurrencyGroup(arrangeName,new() { arrangeGold });

            // Assert
            Assert.NotNull(currencyGroup);
            Assert.Equal(arrangeName, currencyGroup.GroupName);
            Assert.NotNull(currencyGroup.Currencies);
        }

        /// <summary>
        /// Ověří, že přidání více měn do skupiny vede k očekávanému počtu měn v kolekci.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Add_SingleCurrency()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold});
            // Act
            currencyGroup.AddCurrency(arrangeSilver);

            // Assert
            Assert.Contains(arrangeGold, currencyGroup.Currencies);
            Assert.Contains(arrangeSilver, currencyGroup.Currencies);
            Assert.Equal(2, currencyGroup.Currencies.Count);
        }


        /// <summary>
        /// Ověří, že pokus o přidání měny se stejnou úrovní jako existující měna vyvolá <see cref="InvalidOperationException"/>.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Add_DuplicateLevel_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold });
            var sameGold = new SingleCurrency("Another Gold", "ag", 1, 5); // Stejná úroveň jako arrangeGold

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => currencyGroup.AddCurrency(sameGold));
            Assert.Contains($"Měna s úrovní 1 již existuje", exception.Message, System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Ověří, že pokus o přidání null měny vyvolá <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Add_NullCurrency_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName,new(){ arrangeGold });

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => currencyGroup.AddCurrency(null));
            Assert.Equal("currency", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že odebrání existující měny ze skupiny ji úspěšně odebere z kolekce.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Remove()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold ,arrangeSilver});
            // Act
            var result = currencyGroup.RemoveCurrency(arrangeGold);

            // Assert
            Assert.True(result);
            Assert.Single(currencyGroup.Currencies);
            Assert.DoesNotContain(arrangeGold, currencyGroup.Currencies);
            Assert.Contains(arrangeSilver, currencyGroup.Currencies);
        }

        /// <summary>
        /// Ověří, že pokus na odebrání neexistující měny vrátí false.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Remove_NonExistentCurrency_ReturnsFalse()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold });

            // Act
            var result = currencyGroup.RemoveCurrency(arrangeSilver);

            // Assert
            Assert.False(result);
            Assert.Single(currencyGroup.Currencies);
        }

        /// <summary>
        /// Ověří, že pokus na odebrání null měny vyvolá <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_Remove_NullCurrency_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold });

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => currencyGroup.RemoveCurrency(null));
            Assert.Equal("currency", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="CurrencyGroup.SetGroupName(string)"/> správně změní název skupiny.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_SetGroupName()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold });

            // Act
            currencyGroup.SetGroupName("New Name");

            // Assert
            Assert.Equal("New Name", currencyGroup.GroupName);
        }

        /// <summary>
        /// Ověří, že pokus na nastavení prázdného názvu vyvolá <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_SetGroupName_EmptyName_ThrowsException()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => currencyGroup.SetGroupName(""));
            Assert.Equal("newName", exception.ParamName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="CurrencyGroup.GetByLevel(int)"/> vrátí měnu s danou úrovní.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_GetByLevel()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold,arrangeSilver });

            // Act
            var result = currencyGroup.GetByLevel(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(arrangeSilver, result);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="CurrencyGroup.GetByLevel(int)"/> vrátí null pokud měna s danou úrovní neexistuje.
        /// </summary>
        [Fact]
        public void Test_CurrencyGroup_GetByLevel_NotFound()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup(arrangeName, new() { arrangeGold,});

            // Act
            var result = currencyGroup.GetByLevel(5);

            // Assert
            Assert.Null(result);
        }

       
    }
}
