using HoL.Domain.ValueObjects.Anatomi.Body;
using Xunit;

namespace HoL.DomainTest.ObjectValue
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="BodyPartDefense"/>.
    /// Ověřuje vytváření obranných vlastností tělesných částí, manipulaci s atributy, validaci a fluent API.
    /// </summary>
    /// <remarks>
    /// Testovací scénáře:
    /// <list type="number">
    /// <item><description>Vytváření instance - ověření správné inicializace s platnými a hraniční hodnotami</description></item>
    /// <item><description>Validace konstruktoru - ověření vyhazování výjimek při neplatných vstupech</description></item>
    /// <item><description>Metoda SetVital - nastavení vitálnosti, vrácení this, fluent API</description></item>
    /// <item><description>Metoda SetProtected - nastavení ochrany, vrácení this, fluent API</description></item>
    /// <item><description>Metoda SetArmorValue - nastavení brnění, validace, fluent API</description></item>
    /// <item><description>Fluent API řetězení - ověření řetězení více volání a kombinací</description></item>
    /// <item><description>Výchozí hodnoty - ověření správné inicializace atributů</description></item>
    /// </list>
    /// </remarks>
    public class BodyPartDefenseTest
    {
        #region Vytváření instance - Scenario 1

        /// <summary>
        /// Ověří, že nová instance je vytvořena s správnými hodnotami.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_Creation()
        {
            // Arrange
            const int armorValue = 5;

            // Act
            var defense = new BodyPartDefense(armorValue);

            // Assert
            Assert.NotNull(defense);
            Assert.Equal(armorValue, defense.ArmorValue);
            Assert.False(defense.IsVital);
            Assert.False(defense.IsProtected);
        }

        /// <summary>
        /// Ověří, že konstruktor správně inicializuje s nulovou hodnotou brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_CreationWithZeroArmor()
        {
            // Arrange
            const int armorValue = 0;

            // Act
            var defense = new BodyPartDefense(armorValue);

            // Assert
            Assert.Equal(0, defense.ArmorValue);
            Assert.False(defense.IsVital);
            Assert.False(defense.IsProtected);
        }

        /// <summary>
        /// Ověří, že konstruktor správně inicializuje s vysokou hodnotou brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_CreationWithHighArmorValue()
        {
            // Arrange
            const int armorValue = 100;

            // Act
            var defense = new BodyPartDefense(armorValue);

            // Assert
            Assert.Equal(100, defense.ArmorValue);
        }

        /// <summary>
        /// Ověří, že konstruktor správně inicializuje s maximální celočíselnou hodnotou.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_CreationWithMaxValue()
        {
            // Arrange
            const int armorValue = int.MaxValue;

            // Act
            var defense = new BodyPartDefense(armorValue);

            // Assert
            Assert.Equal(int.MaxValue, defense.ArmorValue);
        }

        #endregion

        #region Validace konstruktoru - Scenario 2

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při záporné hodnotě brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_Creation_NegativeArmor_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                new BodyPartDefense(-1));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při minimální záporné hodnotě.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_Creation_MinNegativeArmor_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                new BodyPartDefense(int.MinValue));
        }

        #endregion

        #region Metoda SetVital - Scenario 3

        /// <summary>
        /// Ověří, že metoda SetVital správně nastaví vitálnost na true.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetVital_True()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetVital(true);

            // Assert
            Assert.True(defense.IsVital);
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetVital správně nastaví vitálnost na false.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetVital_False()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetVital(false);

            // Assert
            Assert.False(defense.IsVital);
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetVital vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetVital_ReturnsSelf()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetVital(true);

            // Assert
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetVital může přepnout stav z true na false.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetVital_Toggle()
        {
            // Arrange
            var defense = new BodyPartDefense(5).SetVital(true);
            Assert.True(defense.IsVital);

            // Act
            defense.SetVital(false);

            // Assert
            Assert.False(defense.IsVital);
        }

        #endregion

        #region Metoda SetProtected - Scenario 4

        /// <summary>
        /// Ověří, že metoda SetProtected správně nastaví ochranu na true.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetProtected_True()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetProtected(true);

            // Assert
            Assert.True(defense.IsProtected);
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetProtected správně nastaví ochranu na false.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetProtected_False()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetProtected(false);

            // Assert
            Assert.False(defense.IsProtected);
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetProtected vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetProtected_ReturnsSelf()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetProtected(true);

            // Assert
            Assert.Same(defense, result);
        }

        #endregion

        #region Metoda SetArmorValue - Scenario 5

        /// <summary>
        /// Ověří, že metoda SetArmorValue správně změní hodnotu brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue()
        {
            // Arrange
            var defense = new BodyPartDefense(5);
            const int newValue = 10;

            // Act
            var result = defense.SetArmorValue(newValue);

            // Assert
            Assert.Equal(newValue, defense.ArmorValue);
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetArmorValue vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue_ReturnsSelf()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            var result = defense.SetArmorValue(10);

            // Assert
            Assert.Same(defense, result);
        }

        /// <summary>
        /// Ověří, že metoda SetArmorValue nastaví nulu.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue_Zero()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act
            defense.SetArmorValue(0);

            // Assert
            Assert.Equal(0, defense.ArmorValue);
        }

        /// <summary>
        /// Ověří, že metoda SetArmorValue vyhodí výjimku při záporné hodnotě.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue_NegativeValue_ThrowsException()
        {
            // Arrange
            var defense = new BodyPartDefense(5);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                defense.SetArmorValue(-1));
        }

        /// <summary>
        /// Ověří, že metoda SetArmorValue zachovává stav při výjimce.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue_ExceptionPreservesState()
        {
            // Arrange
            var defense = new BodyPartDefense(5);
            const int originalValue = 5;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                defense.SetArmorValue(-1));
            Assert.Equal(originalValue, defense.ArmorValue);
        }

        /// <summary>
        /// Ověří, že metoda SetArmorValue nastaví vysokou hodnotu.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_SetArmorValue_HighValue()
        {
            // Arrange
            var defense = new BodyPartDefense(5);
            const int highValue = 1000;

            // Act
            defense.SetArmorValue(highValue);

            // Assert
            Assert.Equal(highValue, defense.ArmorValue);
        }

        #endregion

        #region Fluent API řetězení - Scenario 6

        /// <summary>
        /// Ověří, že fluent API umožňuje řetězení více volání metod.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_FluentAPI_Chaining()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(5)
                .SetVital(true)
                .SetProtected(true)
                .SetArmorValue(10);

            // Assert
            Assert.Equal(10, defense.ArmorValue);
            Assert.True(defense.IsVital);
            Assert.True(defense.IsProtected);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje opakované změny atributu.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_FluentAPI_MultipleChanges()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(5)
                .SetArmorValue(10)
                .SetArmorValue(15)
                .SetArmorValue(20);

            // Assert
            Assert.Equal(20, defense.ArmorValue);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje komplexní kombinaci operací.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_FluentAPI_ComplexChaining()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(0)
                .SetVital(false)
                .SetProtected(true)
                .SetArmorValue(5)
                .SetVital(true)
                .SetArmorValue(8)
                .SetProtected(false);

            // Assert
            Assert.Equal(8, defense.ArmorValue);
            Assert.True(defense.IsVital);
            Assert.False(defense.IsProtected);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje toggle vitálnosti.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_FluentAPI_VitalToggle()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(5)
                .SetVital(true)
                .SetVital(false)
                .SetVital(true);

            // Assert
            Assert.True(defense.IsVital);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje toggle ochrany.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_FluentAPI_ProtectedToggle()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(5)
                .SetProtected(true)
                .SetProtected(false);

            // Assert
            Assert.False(defense.IsProtected);
        }

        #endregion

        #region Výchozí hodnoty - Scenario 7

        /// <summary>
        /// Ověří, že atributy jsou správně inicializovány na výchozí hodnoty.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_DefaultValues()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(0);

            // Assert
            Assert.Equal(0, defense.ArmorValue);
            Assert.False(defense.IsVital);
            Assert.False(defense.IsProtected);
        }

        /// <summary>
        /// Ověří, že IsVital je výchozí false i s vysokou hodnotou brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_DefaultVitalFalse()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(100);

            // Assert
            Assert.False(defense.IsVital);
        }

        /// <summary>
        /// Ověří, že IsProtected je výchozí false i s vysokou hodnotou brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_DefaultProtectedFalse()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(100);

            // Assert
            Assert.False(defense.IsProtected);
        }

        #endregion

        #region Kombinované scénáře - Scenario 8

        /// <summary>
        /// Ověří typický scénář vytvoření srdce (vitální část s brnění).
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_HeartDefense()
        {
            // Arrange & Act
            var heartDefense = new BodyPartDefense(3)
                .SetVital(true)
                .SetProtected(true);

            // Assert
            Assert.Equal(3, heartDefense.ArmorValue);
            Assert.True(heartDefense.IsVital);
            Assert.True(heartDefense.IsProtected);
        }

        /// <summary>
        /// Ověří typický scénář vytvoření paže (bez vitálnosti, chráněné).
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_ArmDefense()
        {
            // Arrange & Act
            var armDefense = new BodyPartDefense(5)
                .SetVital(false)
                .SetProtected(true);

            // Assert
            Assert.Equal(5, armDefense.ArmorValue);
            Assert.False(armDefense.IsVital);
            Assert.True(armDefense.IsProtected);
        }

        /// <summary>
        /// Ověří typický scénář vytvoření nechráněné kůže bez brnění.
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_UnarmaredSkin()
        {
            // Arrange & Act
            var skinDefense = new BodyPartDefense(0)
                .SetVital(false)
                .SetProtected(false);

            // Assert
            Assert.Equal(0, skinDefense.ArmorValue);
            Assert.False(skinDefense.IsVital);
            Assert.False(skinDefense.IsProtected);
        }

        /// <summary>
        /// Ověří upgrading defensívy v průběhu času (simulace nasazení vybavení).
        /// </summary>
        [Fact]
        public void Test_BodyPartDefense_EquipmentUpgrade()
        {
            // Arrange
            var defense = new BodyPartDefense(2);

            // Act - Postupné zvýšení brnění (nasazení vybavení)
            defense.SetArmorValue(5);   // Lehký krunýř
            defense.SetArmorValue(8);   // Střední krunýř
            defense.SetArmorValue(12);  // Těžký krunýř

            // Assert
            Assert.Equal(12, defense.ArmorValue);
        }

        #endregion
    }
}
