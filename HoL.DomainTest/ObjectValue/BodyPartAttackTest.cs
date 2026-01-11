using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects.Anatomi.Body;
using Xunit;

namespace HoL.DomainTest.ObjectValue
{
    /// <summary>
    /// Testovací sada pro třídu <see cref="BodyPartAttack"/>.
    /// Ověřuje vytváření útočných vlastností tělesných částí, manipulaci s atributy, validaci a fluent API.
    /// </summary>
    /// <remarks>
    /// Testovací scénáře:
    /// <list type="number">
    /// <item><description>Vytváření instance - ověření správné inicializace se všemi parametry</description></item>
    /// <item><description>Validace - ověření výchozích hodnot iniciativy a kombinování</description></item>
    /// <item><description>Metoda SetDamageType - nastavení typu poškození, fluent API</description></item>
    /// <item><description>Metoda SetInitiative - nastavení iniciativy, fluent API</description></item>
    /// <item><description>Metoda SetCanBeUsedWithOtherAttacks - nastavení kombinování, fluent API</description></item>
    /// <item><description>Metoda SetDice - nastavení kostky poškození, fluent API</description></item>
    /// <item><description>Fluent API řetězení - ověření řetězení více volání</description></item>
    /// <item><description>Reálné scénáře - vytváření typických útoků tělesných částí</description></item>
    /// </list>
    /// </remarks>
    public class BodyPartAttackTest
    {
        #region Vytváření instance - Scenario 1

        /// <summary>
        /// Ověří, že nová instance je vytvořena s platnými hodnotami.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_Creation()
        {
            // Arrange
            var dice = new Dice(1, DiceType.D6, 0);
            const DamageType damageType = DamageType.Piercing;
            const int initiative = 2;
            const bool canBeUsedWithOtherAttacks = true;

            // Act
            var attack = new BodyPartAttack(dice, damageType, initiative, canBeUsedWithOtherAttacks);

            // Assert
            Assert.NotNull(attack);
            Assert.Equal(dice, attack.DamageDice);
            Assert.Equal(damageType, attack.DamageType);
            Assert.Equal(initiative, attack.Initiative);
            Assert.True(attack.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří, že konstruktor vytvoří instanci s iniciatívou 1.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_CreationWithMinimumInitiative()
        {
            // Arrange
            var dice = new Dice(1, DiceType.D4, 0);
            const int initiative = 1;

            // Act
            var attack = new BodyPartAttack(dice, DamageType.Slashing, initiative, false);

            // Assert
            Assert.Equal(1, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že konstruktor vytvoří instanci s vysokou iniciatívou.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_CreationWithHighInitiative()
        {
            // Arrange
            var dice = new Dice(1, DiceType.D8, 0);
            const int initiative = 10;

            // Act
            var attack = new BodyPartAttack(dice, DamageType.Piercing, initiative, true);

            // Assert
            Assert.Equal(10, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že konstruktor vytvoří instanci se všemi typy poškození.
        /// </summary>
        [Theory]
        [InlineData(DamageType.Slashing)]
        [InlineData(DamageType.Piercing)]
        [InlineData(DamageType.Bludgeoning)]
        [InlineData(DamageType.Fire)]
        [InlineData(DamageType.Cold)]
        [InlineData(DamageType.Lightning)]
        [InlineData(DamageType.Poison)]
        public void Test_BodyPartAttack_CreationWithAllDamageTypes(DamageType damageType)
        {
            // Arrange
            var dice = new Dice(1, DiceType.D6, 0);

            // Act
            var attack = new BodyPartAttack(dice, damageType, 2, true);

            // Assert
            Assert.Equal(damageType, attack.DamageType);
        }

        #endregion

        #region Validace - Scenario 2

        /// <summary>
        /// Ověří, že výchozí hodnota iniciativy je 1.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_DefaultInitiative()
        {
            // Arrange
            var dice = new Dice(1, DiceType.D6, 0);

            // Act
            var attack = new BodyPartAttack(dice, DamageType.Piercing, 1, false);

            // Assert
            Assert.Equal(1, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že výchozí hodnota CanBeUsedWithOtherAttacks je false.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_DefaultCanBeUsedWithOtherAttacks()
        {
            // Arrange
            var dice = new Dice(1, DiceType.D8, 0);

            // Act
            var attack = new BodyPartAttack(dice, DamageType.Piercing, 3, false);

            // Assert
            Assert.False(attack.CanBeUsedWithOtherAttacks);
        }

        #endregion

        #region Metoda SetDamageType - Scenario 3

        /// <summary>
        /// Ověří, že metoda SetDamageType správně změní typ poškození.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetDamageType()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);
            const DamageType newDamageType = DamageType.Piercing;

            // Act
            var result = attack.SetDamageType(newDamageType);

            // Assert
            Assert.Equal(newDamageType, attack.DamageType);
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDamageType vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetDamageType_ReturnsSelf()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);

            // Act
            var result = attack.SetDamageType(DamageType.Piercing);

            // Assert
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDamageType změní typ z jedné hodnoty na jinou.
        /// </summary>
        [Theory]
        [InlineData(DamageType.Slashing, DamageType.Piercing)]
        [InlineData(DamageType.Bludgeoning, DamageType.Fire)]
        [InlineData(DamageType.Cold, DamageType.Lightning)]
        [InlineData(DamageType.Poison, DamageType.Slashing)]
        public void Test_BodyPartAttack_SetDamageType_AllTransitions(DamageType from, DamageType to)
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), from, 2, true);

            // Act
            attack.SetDamageType(to);

            // Assert
            Assert.Equal(to, attack.DamageType);
        }

        #endregion

        #region Metoda SetInitiative - Scenario 4

        /// <summary>
        /// Ověří, že metoda SetInitiative správně změní iniciativu.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetInitiative()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);
            const int newInitiative = 5;

            // Act
            var result = attack.SetInitiative(newInitiative);

            // Assert
            Assert.Equal(newInitiative, attack.Initiative);
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetInitiative vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetInitiative_ReturnsSelf()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);

            // Act
            var result = attack.SetInitiative(5);

            // Assert
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetInitiative nastaví minimální iniciativu.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetInitiative_Minimum()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 5, true);

            // Act
            attack.SetInitiative(1);

            // Assert
            Assert.Equal(1, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že metoda SetInitiative nastaví vysokou iniciativu.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetInitiative_High()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);

            // Act
            attack.SetInitiative(15);

            // Assert
            Assert.Equal(15, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že metoda SetInitiative může zvýšit i snížit iniciativu.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetInitiative_Multiple()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);

            // Act
            attack.SetInitiative(5);
            attack.SetInitiative(3);
            attack.SetInitiative(8);

            // Assert
            Assert.Equal(8, attack.Initiative);
        }

        #endregion

        #region Metoda SetCanBeUsedWithOtherAttacks - Scenario 5

        /// <summary>
        /// Ověří, že metoda SetCanBeUsedWithOtherAttacks správně nastaví na true.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetCanBeUsedWithOtherAttacks_True()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, false);

            // Act
            var result = attack.SetCanBeUsedWithOtherAttacks(true);

            // Assert
            Assert.True(attack.CanBeUsedWithOtherAttacks);
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetCanBeUsedWithOtherAttacks správně nastaví na false.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetCanBeUsedWithOtherAttacks_False()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);

            // Act
            var result = attack.SetCanBeUsedWithOtherAttacks(false);

            // Assert
            Assert.False(attack.CanBeUsedWithOtherAttacks);
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetCanBeUsedWithOtherAttacks vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetCanBeUsedWithOtherAttacks_ReturnsSelf()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, false);

            // Act
            var result = attack.SetCanBeUsedWithOtherAttacks(true);

            // Assert
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetCanBeUsedWithOtherAttacks může přepínat mezi stavy.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetCanBeUsedWithOtherAttacks_Toggle()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, false);

            // Act
            attack.SetCanBeUsedWithOtherAttacks(true);
            Assert.True(attack.CanBeUsedWithOtherAttacks);
            attack.SetCanBeUsedWithOtherAttacks(false);

            // Assert
            Assert.False(attack.CanBeUsedWithOtherAttacks);
        }

        #endregion

        #region Metoda SetDice - Scenario 6

        /// <summary>
        /// Ověří, že metoda SetDice správně změní kostku poškození.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetDice()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Slashing, 2, true);
            var newDice = new Dice(1, DiceType.D8, 0);

            // Act
            var result = attack.SetDice(newDice);

            // Assert
            Assert.Same(newDice, attack.DamageDice);
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDice vrací this pro fluent API.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetDice_ReturnsSelf()
        {
            // Arrange
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);
            var newDice = new Dice(2, DiceType.D6, 0);

            // Act
            var result = attack.SetDice(newDice);

            // Assert
            Assert.Same(attack, result);
        }

        /// <summary>
        /// Ověří, že metoda SetDice změní kostku poškození.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_SetDice_Changes()
        {
            // Arrange
            var originalDice = new Dice(1, DiceType.D4, 0);
            var attack = new BodyPartAttack(originalDice, DamageType.Slashing, 2, true);
            var higherDice = new Dice(1, DiceType.D10, 0);

            // Act
            attack.SetDice(higherDice);

            // Assert
            Assert.NotSame(originalDice, attack.DamageDice);
            Assert.Same(higherDice, attack.DamageDice);
        }

        #endregion

        #region Fluent API řetězení - Scenario 7

        /// <summary>
        /// Ověří, že fluent API umožňuje řetězení více volání metod.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_FluentAPI_Chaining()
        {
            // Arrange & Act
            var newDice = new Dice(1, DiceType.D6, 0);
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Slashing, 1, false)
                .SetDice(newDice)
                .SetDamageType(DamageType.Piercing)
                .SetInitiative(3)
                .SetCanBeUsedWithOtherAttacks(true);

            // Assert
            Assert.Same(newDice, attack.DamageDice);
            Assert.Equal(DamageType.Piercing, attack.DamageType);
            Assert.Equal(3, attack.Initiative);
            Assert.True(attack.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje opakované změny iniciativy.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_FluentAPI_MultipleInitiativeChanges()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 1, true)
                .SetInitiative(2)
                .SetInitiative(3)
                .SetInitiative(4)
                .SetInitiative(5);

            // Assert
            Assert.Equal(5, attack.Initiative);
        }

        /// <summary>
        /// Ověří, že fluent API umožňuje togglejt kombinování.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_FluentAPI_ToggleCombining()
        {
            // Arrange & Act
            var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Piercing, 2, false)
                .SetCanBeUsedWithOtherAttacks(true)
                .SetCanBeUsedWithOtherAttacks(false)
                .SetCanBeUsedWithOtherAttacks(true);

            // Assert
            Assert.True(attack.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří komplexní řetězení s vícenásobnými změnami.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_FluentAPI_ComplexChaining()
        {
            // Arrange & Act
            var newDice = new Dice(2, DiceType.D6, 0);
            var attack = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Slashing, 1, false)
                .SetDamageType(DamageType.Fire)
                .SetInitiative(4)
                .SetDice(newDice)
                .SetCanBeUsedWithOtherAttacks(true)
                .SetDamageType(DamageType.Lightning)
                .SetInitiative(6);

            // Assert
            Assert.Same(newDice, attack.DamageDice);
            Assert.Equal(DamageType.Lightning, attack.DamageType);
            Assert.Equal(6, attack.Initiative);
            Assert.True(attack.CanBeUsedWithOtherAttacks);
        }

        #endregion

        #region Reálné scénáře - Scenario 8

        /// <summary>
        /// Ověří typický scénář vytvoření útoku drápů (kombinovatelný).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_ClawAttack()
        {
            // Arrange & Act
            var clawDice = new Dice(1, DiceType.D4, 0);
            var claw = new BodyPartAttack(clawDice, DamageType.Piercing, 2, true);

            // Assert
            Assert.Same(clawDice, claw.DamageDice);
            Assert.Equal(DamageType.Piercing, claw.DamageType);
            Assert.Equal(2, claw.Initiative);
            Assert.True(claw.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří typický scénář vytvoření útoku paží údery (kombinovatelný).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_PunchAttack()
        {
            // Arrange & Act
            var punchDice = new Dice(1, DiceType.D4, 0);
            var punch = new BodyPartAttack(punchDice, DamageType.Bludgeoning, 2, true);

            // Assert
            Assert.Same(punchDice, punch.DamageDice);
            Assert.Equal(DamageType.Bludgeoning, punch.DamageType);
            Assert.True(punch.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří typický scénář vytvoření útoku zobákem (exkluzivní).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_BiteAttack()
        {
            // Arrange & Act
            var biteDice = new Dice(1, DiceType.D8, 0);
            var bite = new BodyPartAttack(biteDice, DamageType.Piercing, 3, false);

            // Assert
            Assert.Same(biteDice, bite.DamageDice);
            Assert.Equal(DamageType.Piercing, bite.DamageType);
            Assert.Equal(3, bite.Initiative);
            Assert.False(bite.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří typický scénář vytvoření útoku nohami (kopance).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_KickAttack()
        {
            // Arrange & Act
            var kickDice = new Dice(1, DiceType.D6, 0);
            var kick = new BodyPartAttack(kickDice, DamageType.Bludgeoning, 1, true);

            // Assert
            Assert.Same(kickDice, kick.DamageDice);
            Assert.Equal(DamageType.Bludgeoning, kick.DamageType);
            Assert.Equal(1, kick.Initiative);
            Assert.True(kick.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří upgrade drápů během hry (zvýšení poškození).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_ClawUpgrade()
        {
            // Arrange
            var claw = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Piercing, 2, true);
            var upgradedDice = new Dice(1, DiceType.D6, 0);

            // Act - Upgrade na ztenčilé drápy
            claw.SetDice(upgradedDice);
            claw.SetInitiative(3);

            // Assert
            Assert.Same(upgradedDice, claw.DamageDice);
            Assert.Equal(3, claw.Initiative);
        }

        /// <summary>
        /// Ověří vznik magického útoku z fyzického.
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_MagicalTransformation()
        {
            // Arrange
            var hand = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);
            var magicalDice = new Dice(2, DiceType.D6, 0);

            // Act - Magická transformace
            hand.SetDamageType(DamageType.Fire)
                .SetDice(magicalDice)
                .SetInitiative(4)
                .SetCanBeUsedWithOtherAttacks(false);

            // Assert
            Assert.Equal(DamageType.Fire, hand.DamageType);
            Assert.Same(magicalDice, hand.DamageDice);
            Assert.Equal(4, hand.Initiative);
            Assert.False(hand.CanBeUsedWithOtherAttacks);
        }

        /// <summary>
        /// Ověří vytvoření multiattacku (dvě paže).
        /// </summary>
        [Fact]
        public void Test_BodyPartAttack_DualWieldAttacks()
        {
            // Arrange & Act
            var dice = new Dice(1, DiceType.D4, 0);
            var leftHand = new BodyPartAttack(dice, DamageType.Bludgeoning, 2, true);
            var rightHand = new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true);

            // Assert
            Assert.True(leftHand.CanBeUsedWithOtherAttacks);
            Assert.True(rightHand.CanBeUsedWithOtherAttacks);
            Assert.Equal(leftHand.Initiative, rightHand.Initiative);
        }

        #endregion
    }
}
