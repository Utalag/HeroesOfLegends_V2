using HoL.Domain.ValueObjects;
using Xunit;

namespace HoL.DomainTest.ObjectValue
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="SpecialAbilities"/>.
    /// Ověřuje vytváření speciálních schopností, manipulaci s atributy, validaci a fluent API.
    /// </summary>
    /// <remarks>
    /// Testovací scénáře:
    /// <list type="number">
    /// <item><description>Vytváření instance - ověření správné inicializace s platnými a prázdnými hodnotami</description></item>
    /// <item><description>Validace konstruktoru - ověření vyhazování výjimek při neplatných vstupech</description></item>
    /// <item><description>Metoda WithName - nastavení názvu, validace, fluent API</description></item>
    /// <item><description>Metoda WithDescription - nastavení popisu, zpracování null, fluent API</description></item>
    /// <item><description>Fluent API řetězení - ověření řetězení více volání a opakovaných změn</description></item>
    /// <item><description>Robustnost - zpracování speciálních znaků a dlouhých textů</description></item>
    /// <item><description>Výchozí hodnoty - ověření správné inicializace atributů</description></item>
    /// </list>
    /// </remarks>
    public class SpecialAbilitiesTest
    {
        #region Vytváření instance - Scenario 1

        /// <summary>
        /// Ověří, že nová instance <see cref="SpecialAbilities"/> je vytvořena s správnými hodnotami.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_Creation()
        {
            // Arrange
            const string name = "Magická rezistence";
            const string description = "Schopnost odolávat magickým útokům";

            // Act
            var ability = new SpecialAbilities(name, description);

            // Assert
            Assert.NotNull(ability);
            Assert.Equal(name, ability.AbilityName);
            Assert.Equal(description, ability.AbilityDescription);
        }

        /// <summary>
        /// Ověří, že konstruktor správně inicializuje schopnost s prázdným popisem.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_CreationWithEmptyDescription()
        {
            // Arrange
            const string name = "Rychlý útok";
            const string description = "";

            // Act
            var ability = new SpecialAbilities(name, description);

            // Assert
            Assert.Equal(name, ability.AbilityName);
            Assert.Equal(description, ability.AbilityDescription);
        }

        #endregion

        #region Validace konstruktoru - Scenario 2

        /// <summary>
        /// Ověří, že konstruktor vyhodí <see cref="ArgumentException"/> při prázdném názvu.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_Creation_EmptyName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new SpecialAbilities("", "Popis"));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí <see cref="ArgumentException"/> při null názvu.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_Creation_NullName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new SpecialAbilities(null, "Popis"));
        }

        #endregion

        #region Metoda WithName - Scenario 3

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithName"/> správně změní název schopnosti.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithName()
        {
            // Arrange
            var ability = new SpecialAbilities("Původní název", "Popis");
            const string newName = "Nový název";

            // Act
            var result = ability.WithName(newName);

            // Assert
            Assert.Equal(newName, ability.AbilityName);
            Assert.Equal(newName, result.AbilityName);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithName"/> vrací `this` pro fluent API.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithName_ReturnsSelf()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "Popis");

            // Act
            var result = ability.WithName("Nový název");

            // Assert
            Assert.Same(ability, result);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithName"/> vyhodí <see cref="ArgumentException"/> při prázdném názvu.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithName_EmptyName_ThrowsException()
        {
            // Arrange
            var ability = new SpecialAbilities("Původní název", "Popis");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ability.WithName(""));
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithName"/> vyhodí <see cref="ArgumentException"/> při null názvu.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithName_NullName_ThrowsException()
        {
            // Arrange
            var ability = new SpecialAbilities("Původní název", "Popis");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ability.WithName(null));
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithName"/> vyhodí <see cref="ArgumentException"/> při samé mezeře.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithName_WhitespaceOnly_ThrowsException()
        {
            // Arrange
            var ability = new SpecialAbilities("Původní název", "Popis");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ability.WithName("   "));
        }

        #endregion

        #region Metoda WithDescription - Scenario 4

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithDescription"/> správně změní popis schopnosti.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithDescription()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "Původní popis");
            const string newDescription = "Nový popis";

            // Act
            var result = ability.WithDescription(newDescription);

            // Assert
            Assert.Equal(newDescription, ability.AbilityDescription);
            Assert.Equal(newDescription, result.AbilityDescription);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithDescription"/> vrací `this` pro fluent API.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithDescription_ReturnsSelf()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "Popis");

            // Act
            var result = ability.WithDescription("Nový popis");

            // Assert
            Assert.Same(ability, result);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithDescription"/> v pořádku zpracuje null hodnotu (konvertuje na prázdný string).
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithDescription_NullDescription()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "Popis");

            // Act
            var result = ability.WithDescription(null);

            // Assert
            Assert.Equal(string.Empty, ability.AbilityDescription);
            Assert.Same(ability, result);
        }

        /// <summary>
        /// Ověří, že metoda <see cref="SpecialAbilities.WithDescription"/> akceptuje prázdný string.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_WithDescription_EmptyString()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "Původní popis");

            // Act
            var result = ability.WithDescription("");

            // Assert
            Assert.Equal(string.Empty, ability.AbilityDescription);
            Assert.Same(ability, result);
        }

        #endregion

        #region Fluent API řetězení - Scenario 5

        /// <summary>
        /// Ověří, že fluent API umožňuje řetězení více volání metod.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_FluentAPI_Chaining()
        {
            // Arrange & Act
            var ability = new SpecialAbilities("Původní název", "Původní popis")
                .WithName("Nový název")
                .WithDescription("Nový popis");

            // Assert
            Assert.Equal("Nový název", ability.AbilityName);
            Assert.Equal("Nový popis", ability.AbilityDescription);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje opakované změny stejného atributu.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_FluentAPI_MultipleChanges()
        {
            // Arrange & Act
            var ability = new SpecialAbilities("Název 1", "Popis 1")
                .WithName("Název 2")
                .WithName("Název 3")
                .WithDescription("Popis 2")
                .WithDescription("Popis 3");

            // Assert
            Assert.Equal("Název 3", ability.AbilityName);
            Assert.Equal("Popis 3", ability.AbilityDescription);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje komplexní kombinaci operací.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_FluentAPI_ComplexChaining()
        {
            // Arrange & Act
            var ability = new SpecialAbilities("Magická rezistence", "")
                .WithDescription("Schopnost odolávat magickým útokům")
                .WithName("Vylepšená magická rezistence")
                .WithDescription("Pokročilá schopnost odolávat všem druhům magie");

            // Assert
            Assert.Equal("Vylepšená magická rezistence", ability.AbilityName);
            Assert.Equal("Pokročilá schopnost odolávat všem druhům magie", ability.AbilityDescription);
        }

        #endregion

        #region Robustnost - Scenario 6

        /// <summary>
        /// Ověří, že speciální znaky v názvu a popisu jsou správně zpracovány.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_SpecialCharacters()
        {
            // Arrange
            const string nameWithSpecialChars = "Útok + Obrana (levá ruka)";
            const string descriptionWithSpecialChars = "Zvýší schopnost o 50% — magické & fyzické";

            // Act
            var ability = new SpecialAbilities(nameWithSpecialChars, descriptionWithSpecialChars)
                .WithName("Útok [pokročilý] → +20%")
                .WithDescription("Složitý popis: part1, part2; part3");

            // Assert
            Assert.Equal("Útok [pokročilý] → +20%", ability.AbilityName);
            Assert.Equal("Složitý popis: part1, part2; part3", ability.AbilityDescription);
        }

        /// <summary>
        /// Ověří, že dlouhé texty jsou správně zpracovány.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_LongTexts()
        {
            // Arrange
            var longName = new string('A', 1000);
            var longDescription = new string('B', 5000);

            // Act
            var ability = new SpecialAbilities(longName, longDescription);

            // Assert
            Assert.Equal(longName, ability.AbilityName);
            Assert.Equal(longDescription, ability.AbilityDescription);
        }

        #endregion

        #region Výchozí hodnoty - Scenario 7

        /// <summary>
        /// Ověří, že atributy jsou správně inicializovány na výchozí hodnoty.
        /// </summary>
        [Fact]
        public void Test_SpecialAbilities_DefaultValues()
        {
            // Arrange
            var ability = new SpecialAbilities("Název", "");

            // Assert
            Assert.Equal("Název", ability.AbilityName);
            Assert.Equal(string.Empty, ability.AbilityDescription);
        }

        #endregion
    }
}
