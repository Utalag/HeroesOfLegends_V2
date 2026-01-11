using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects.Anatomi.Body;
using Xunit;

namespace HoL.DomainTest.ObjectValue
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="BodyPart"/>.
    /// Ověřuje vytváření tělesných částí, manipulaci s atributy, validaci a fluent API.
    /// </summary>
    /// <remarks>
    /// Testovací scénáře:
    /// <list type="number">
    /// <item><description>Vytváření instance - ověření správné inicializace s povinnými parametry</description></item>
    /// <item><description>Validace konstruktoru - ověření vyhazování výjimek při neplatných vstupech</description></item>
    /// <item><description>Metoda SetName - nastavení názvu, validace, fluent API</description></item>
    /// <item><description>Metoda SetType - nastavení typu, fluent API</description></item>
    /// <item><description>Metoda SetQuantity - nastavení počtu, validace, fluent API</description></item>
    /// <item><description>Metoda SetFunction - nastavení funkce, validace, fluent API</description></item>
    /// <item><description>Metoda SetAppearance - nastavení vzhledu, fluent API</description></item>
    /// <item><description>Metoda SetIsMagical - nastavení magičnosti, fluent API</description></item>
    /// <item><description>Metoda SetAttack - nastavení útočných vlastností, fluent API</description></item>
    /// <item><description>Metoda SetDefense - nastavení obranných vlastností, fluent API</description></item>
    /// <item><description>Fluent API řetězení - ověření řetězení více volání</description></item>
    /// <item><description>Reálné scénáře - vytváření typických tělesných částí</description></item>
    /// </list>
    /// </remarks>
    public class BodyPartTest
    {
        #region Vytváření instance - Scenario 1

        /// <summary>
        /// Ověří, že nová instance je vytvořena s správnými hodnotami.
        /// </summary>
        [Fact]
        public void Test_BodyPart_Creation()
        {
            // Arrange
            const string name = "hlava";
            const BodyPartType type = BodyPartType.Head;
            const int quantity = 1;

            // Act
            var part = new BodyPart(name, type, quantity);

            // Assert
            Assert.NotNull(part);
            Assert.Equal(name, part.Name);
            Assert.Equal(type, part.BodyPartCategory);
            Assert.Equal(quantity, part.Quantity);
            Assert.Equal(string.Empty, part.Function);
            Assert.Null(part.Attack);
            Assert.Null(part.Defense);
            Assert.Equal(string.Empty, part.Appearance);
            Assert.False(part.IsMagical);
        }

        /// <summary>
        /// Ověří, že konstruktor vytvoří instanci s více kusy.
        /// </summary>
        [Fact]
        public void Test_BodyPart_CreationWithMultipleQuantity()
        {
            // Arrange
            const string name = "noha";
            const int quantity = 4;

            // Act
            var part = new BodyPart(name, BodyPartType.Leg, quantity);

            // Assert
            Assert.Equal(quantity, part.Quantity);
        }

        /// <summary>
        /// Ověří vytvoření instance se všemi typy tělesných částí.
        /// </summary>
        [Theory]
        [InlineData(BodyPartType.Head)]
        [InlineData(BodyPartType.Arm)]
        [InlineData(BodyPartType.Leg)]
        [InlineData(BodyPartType.Wing)]
        [InlineData(BodyPartType.Tail)]
        [InlineData(BodyPartType.Claw)]
        public void Test_BodyPart_CreationWithAllTypes(BodyPartType type)
        {
            // Arrange & Act
            var part = new BodyPart("část", type, 1);

            // Assert
            Assert.Equal(type, part.BodyPartCategory);
        }

        #endregion

        #region Validace konstruktoru - Scenario 2

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při prázdném názvu.
        /// </summary>
        [Fact]
        public void Test_BodyPart_Creation_EmptyName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new BodyPart("", BodyPartType.Head, 1));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při null názvu.
        /// </summary>
        [Fact]
        public void Test_BodyPart_Creation_NullName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new BodyPart(null, BodyPartType.Head, 1));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při samých mezerách v názvu.
        /// </summary>
        [Fact]
        public void Test_BodyPart_Creation_WhitespaceName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new BodyPart("   ", BodyPartType.Head, 1));
        }

        /// <summary>
        /// Ověří, že konstruktor vyhodí výjimku při nulové či záporné количестве.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Test_BodyPart_Creation_InvalidQuantity_ThrowsException(int invalidQuantity)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new BodyPart("hlava", BodyPartType.Head, invalidQuantity));
        }

        #endregion

        #region Metoda SetName - Scenario 3

        /// <summary>
        /// Ověří, že metoda SetName správně změní název.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetName()
        {
            // Arrange
            var part = new BodyPart("stará název", BodyPartType.Head, 1);
            const string newName = "nový název";

            // Act
            var result = part.SetName(newName);

            // Assert
            Assert.Equal(newName, part.Name);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetName vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetName_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("název", BodyPartType.Head, 1);

            // Act
            var result = part.SetName("nový název");

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetName vyhodí výjimku při prázdném názvu.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetName_EmptyName_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("název", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetName(""));
        }

        /// <summary>
        /// Ověří, že metoda SetName vyhodí výjimku při null.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetName_NullName_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("název", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetName(null));
        }

        #endregion

        #region Metoda SetType - Scenario 4

        /// <summary>
        /// Ověří, že metoda SetType správně změní typ.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetType()
        {
            // Arrange
            var part = new BodyPart("část", BodyPartType.Head, 1);
            const BodyPartType newType = BodyPartType.Arm;

            // Act
            var result = part.SetType(newType);

            // Assert
            Assert.Equal(newType, part.BodyPartCategory);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetType vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetType_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("část", BodyPartType.Head, 1);

            // Act
            var result = part.SetType(BodyPartType.Claw);

            // Assert
            Assert.Same(part, result);
        }

        #endregion

        #region Metoda SetQuantity - Scenario 5

        /// <summary>
        /// Ověří, že metoda SetQuantity správně změní počet.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetQuantity()
        {
            // Arrange
            var part = new BodyPart("noha", BodyPartType.Leg, 2);
            const int newQuantity = 4;

            // Act
            var result = part.SetQuantity(newQuantity);

            // Assert
            Assert.Equal(newQuantity, part.Quantity);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetQuantity vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetQuantity_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("noha", BodyPartType.Leg, 2);

            // Act
            var result = part.SetQuantity(4);

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetQuantity nastaví minimální počet.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetQuantity_Minimum()
        {
            // Arrange
            var part = new BodyPart("část", BodyPartType.Head, 2);

            // Act
            part.SetQuantity(1);

            // Assert
            Assert.Equal(1, part.Quantity);
        }

        /// <summary>
        /// Ověří, že metoda SetQuantity vyhodí výjimku při nule.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetQuantity_Zero_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("část", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetQuantity(0));
        }

        /// <summary>
        /// Ověří, že metoda SetQuantity vyhodí výjimku při záporném čísle.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetQuantity_Negative_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("část", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetQuantity(-5));
        }

        #endregion

        #region Metoda SetFunction - Scenario 6

        /// <summary>
        /// Ověří, že metoda SetFunction správně nastaví funkci.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetFunction()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);
            const string newFunction = "vidění";

            // Act
            var result = part.SetFunction(newFunction);

            // Assert
            Assert.Equal(newFunction, part.Function);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetFunction vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetFunction_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);

            // Act
            var result = part.SetFunction("vidění");

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetFunction vyhodí výjimku při prázdné funkci.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetFunction_Empty_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetFunction(""));
        }

        /// <summary>
        /// Ověří, že metoda SetFunction vyhodí výjimku při null.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetFunction_Null_ThrowsException()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => part.SetFunction(null));
        }

        #endregion

        #region Metoda SetAppearance - Scenario 7

        /// <summary>
        /// Ověří, že metoda SetAppearance správně nastaví vzhled.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAppearance()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);
            const string appearance = "modré peří se zlatými okraji";

            // Act
            var result = part.SetAppearance(appearance);

            // Assert
            Assert.Equal(appearance, part.Appearance);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetAppearance vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAppearance_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);

            // Act
            var result = part.SetAppearance("modré peří");

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetAppearance akceptuje prázdný string.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAppearance_Empty()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);

            // Act
            part.SetAppearance("");

            // Assert
            Assert.Equal(string.Empty, part.Appearance);
        }

        #endregion

        #region Metoda SetIsMagical - Scenario 8

        /// <summary>
        /// Ověří, že metoda SetIsMagical správně nastaví magičnost na true.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetIsMagical_True()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);

            // Act
            var result = part.SetIsMagical(true);

            // Assert
            Assert.True(part.IsMagical);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetIsMagical správně nastaví magičnost na false.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetIsMagical_False()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);
            part.SetIsMagical(true);

            // Act
            var result = part.SetIsMagical(false);

            // Assert
            Assert.False(part.IsMagical);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetIsMagical vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetIsMagical_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("křídlo", BodyPartType.Wing, 1);

            // Act
            var result = part.SetIsMagical(true);

            // Assert
            Assert.Same(part, result);
        }

        #endregion

        #region Metoda SetAttack - Scenario 9

        /// <summary>
        /// Ověří, že metoda SetAttack správně nastaví útočné vlastnosti.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAttack()
        {
            // Arrange
            var part = new BodyPart("paž", BodyPartType.Arm, 1);
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);

            // Act
            var result = part.SetAttack(attack);

            // Assert
            Assert.Same(attack, part.Attack);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetAttack vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAttack_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("paž", BodyPartType.Arm, 1);
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);

            // Act
            var result = part.SetAttack(attack);

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetAttack akceptuje null.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetAttack_Null()
        {
            // Arrange
            var part = new BodyPart("paž", BodyPartType.Arm, 1);
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);
            part.SetAttack(attack);

            // Act
            part.SetAttack(null);

            // Assert
            Assert.Null(part.Attack);
        }

        #endregion

        #region Metoda SetDefense - Scenario 10

        /// <summary>
        /// Ověří, že metoda SetDefense správně nastaví obranné vlastnosti.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetDefense()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);
            var defense = new BodyPartDefense(5);

            // Act
            var result = part.SetDefense(defense);

            // Assert
            Assert.Same(defense, part.Defense);
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDefense vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetDefense_ReturnsSelf()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);
            var defense = new BodyPartDefense(5);

            // Act
            var result = part.SetDefense(defense);

            // Assert
            Assert.Same(part, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDefense akceptuje null.
        /// </summary>
        [Fact]
        public void Test_BodyPart_SetDefense_Null()
        {
            // Arrange
            var part = new BodyPart("hlava", BodyPartType.Head, 1);
            var defense = new BodyPartDefense(5);
            part.SetDefense(defense);

            // Act
            part.SetDefense(null);

            // Assert
            Assert.Null(part.Defense);
        }

        #endregion

        #region Fluent API řetězení - Scenario 11

        /// <summary>
        /// Ověří, že fluent API umožňuje řetězení více volání.
        /// </summary>
        [Fact]
        public void Test_BodyPart_FluentAPI_Chaining()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);
            var defense = new BodyPartDefense(3);
            
            var part = new BodyPart("paž", BodyPartType.Arm, 1)
                .SetName("levá paž")
                .SetFunction("úchop a manipulace")
                .SetAttack(attack)
                .SetDefense(defense)
                .SetAppearance("silná a svalnatá");

            // Assert
            Assert.Equal("levá paž", part.Name);
            Assert.Equal("úchop a manipulace", part.Function);
            Assert.Same(attack, part.Attack);
            Assert.Same(defense, part.Defense);
            Assert.Equal("silná a svalnatá", part.Appearance);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje změnu typu a množství.
        /// </summary>
        [Fact]
        public void Test_BodyPart_FluentAPI_TypeAndQuantity()
        {
            // Arrange & Act
            var part = new BodyPart("část", BodyPartType.Arm, 1)
                .SetType(BodyPartType.Claw)
                .SetQuantity(2)
                .SetType(BodyPartType.Arm)
                .SetQuantity(3);

            // Assert
            Assert.Equal(BodyPartType.Arm, part.BodyPartCategory);
            Assert.Equal(3, part.Quantity);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje komplexní konfiguraci.
        /// </summary>
        [Fact]
        public void Test_BodyPart_FluentAPI_ComplexChaining()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 3, false);
            var defense = new BodyPartDefense(4).SetVital(true);
            
            var part = new BodyPart("hlava", BodyPartType.Head, 1)
                .SetFunction("vidění a smyslové vnímání")
                .SetIsMagical(true)
                .SetAppearance("zelené šupiny se zlatými okraji")
                .SetAttack(attack)
                .SetDefense(defense);

            // Assert
            Assert.Equal("hlava", part.Name);
            Assert.Equal("vidění a smyslové vnímání", part.Function);
            Assert.True(part.IsMagical);
            Assert.Equal("zelené šupiny se zlatými okraji", part.Appearance);
            Assert.NotNull(part.Attack);
            Assert.NotNull(part.Defense);
        }

        #endregion

        #region Reálné scénáře - Scenario 12

        /// <summary>
        /// Ověří vytvoření hlavy se všemi vlastnostmi.
        /// </summary>
        [Fact]
        public void Test_BodyPart_HeadScenario()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(5).SetVital(true);
            var head = new BodyPart("hlava", BodyPartType.Head, 1)
                .SetFunction("vidění a smyslové vnímání")
                .SetDefense(defense)
                .SetAppearance("zelené oči");

            // Assert
            Assert.Equal("hlava", head.Name);
            Assert.Equal(BodyPartType.Head, head.BodyPartCategory);
            Assert.Equal(1, head.Quantity);
            Assert.NotNull(head.Defense);
            Assert.Null(head.Attack);
        }

        /// <summary>
        /// Ověří vytvoření paže s útočnými a obranými vlastnostmi.
        /// </summary>
        [Fact]
        public void Test_BodyPart_ArmScenario()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);
            var defense = new BodyPartDefense(3);
            
            var arm = new BodyPart("levá paž", BodyPartType.Arm, 1)
                .SetName("levá paž")
                .SetFunction("úchop a manipulace")
                .SetAttack(attack)
                .SetDefense(defense);

            // Assert
            Assert.Equal("levá paž", arm.Name);
            Assert.Equal(BodyPartType.Arm, arm.BodyPartCategory);
            Assert.NotNull(arm.Attack);
            Assert.NotNull(arm.Defense);
        }

        /// <summary>
        /// Ověří vytvoření nohou (více kusů).
        /// </summary>
        [Fact]
        public void Test_BodyPart_LegsScenario()
        {
            // Arrange & Act
            var defense = new BodyPartDefense(2);
            var legs = new BodyPart("nohy", BodyPartType.Leg, 4)
                .SetFunction("chůze a běh")
                .SetDefense(defense);

            // Assert
            Assert.Equal(4, legs.Quantity);
            Assert.Equal("chůze a běh", legs.Function);
            Assert.NotNull(legs.Defense);
        }

        /// <summary>
        /// Ověří vytvoření magického křídla.
        /// </summary>
        [Fact]
        public void Test_BodyPart_MagicalWingScenario()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 3, false);
            
            var wing = new BodyPart("levé křídlo", BodyPartType.Wing, 1)
                .SetFunction("let")
                .SetAppearance("irridescenční modro-fialové peří")
                .SetAttack(attack)
                .SetIsMagical(true);

            // Assert
            Assert.True(wing.IsMagical);
            Assert.Equal("irridescenční modro-fialové peří", wing.Appearance);
            Assert.Equal("let", wing.Function);
            Assert.NotNull(wing.Attack);
        }

        /// <summary>
        /// Ověří vytvoření rogů jako zbraně.
        /// </summary>
        [Fact]
        public void Test_BodyPart_HornsScenario()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Piercing, 2, false);
            
            var horns = new BodyPart("rohy", BodyPartType.Horn, 2)
                .SetFunction("útok")
                .SetAttack(attack)
                .SetAppearance("zakřivené černé rohy");

            // Assert
            Assert.Equal(2, horns.Quantity);
            Assert.Equal(BodyPartType.Horn, horns.BodyPartCategory);
            Assert.NotNull(horns.Attack);
        }

        /// <summary>
        /// Ověří upgrade části těla během hry.
        /// </summary>
        [Fact]
        public void Test_BodyPart_UpgradeScenario()
        {
            // Arrange
            var claw = new BodyPart("levý drápek", BodyPartType.Claw, 1)
                .SetFunction("útok")
                .SetAttack(new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Piercing, 1, true));

            // Act - Upgrade při zvýšení levelu
            var strongerAttack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Piercing, 2, true);
            claw.SetAttack(strongerAttack);

            // Assert
            Assert.Same(strongerAttack, claw.Attack);
        }

        #endregion
    }
}
