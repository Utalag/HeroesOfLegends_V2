using HoL.Domain.Builders;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using Xunit;

namespace HoL.DomainTest.Builders
{
    /// <summary>
    /// Testovací sada pro RaceBuilder, včetně nové funkcionalityo editace existujících ras.
    /// </summary>
    public class RaceBuilderTest
    {
        #region Vytváření nové rasy - Scenario 1

        /// <summary>
        /// Ověří, že builder vytvoří novou rasu s povinnými vlastnostmi.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_CreateNewRace()
        {
            // Arrange
            var bodyDims = new BodyDimension(RaceSize.B);

            // Act
            Race race = new RaceBuilder()
                .WithName("Elf")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(bodyDims)
                .Build();

            // Assert
            Assert.NotNull(race);
            Assert.Equal("Elf", race.RaceName);
            Assert.Equal(RaceCategory.Humanoid, race.RaceCategory);
            Assert.Same(bodyDims, race.BodyDimensins);
        }

        /// <summary>
        /// Ověří, že builder vytvoří rasu s fluent API řetězením.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FluentAPI()
        {
            // Arrange
            var bodyDims = new BodyDimension(RaceSize.B);

            // Act
            var race = new RaceBuilder()
                .WithName("Skřet")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(bodyDims)
                .WithDescription("Silní a agresivní tvorové", "Starobylá rasa...")
                .WithBaseInitiative(3)
                .WithBaseXP(150)
                .Build();

            // Assert
            Assert.Equal("Skřet", race.RaceName);
            Assert.Equal("Silní a agresivní tvorové", race.RaceDescription);
            Assert.Equal(3, race.BaseInitiative);
            Assert.Equal(150, race.BaseXP);
        }

        #endregion

        #region Editace existující rasy - Scenario 2

        /// <summary>
        /// Ověří, že builder může načíst existující rasu a vrátit stejnou instanci.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_ReturnsSameInstance()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Elf")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.B))
                .WithDescription("Elfové", "Historie...")
                .Build();

            var originalId = originalRace.Id;

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .WithDescription("Upravené elfové", "Nová historie")
                .Build();

            // Assert
            Assert.Same(originalRace, editedRace);
            Assert.Equal(originalId, editedRace.Id);
        }

        /// <summary>
        /// Ověří, že FromRace načte všechny vlastnosti z existující rasy.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_LoadsAllProperties()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Goblin")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.A))
                .WithDescription("Malé tvory", "Neznámý původ")
                .WithConviction(ConvictionType.Evil)
                .WithZSM(2)
                .WithDomesticationValue(3)
                .WithBaseInitiative(4)
                .WithBaseXP(75)
                .WithFightingSpirit(1)
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace).Build();

            // Assert
            Assert.Equal(originalRace.RaceName, editedRace.RaceName);
            Assert.Equal(originalRace.RaceCategory, editedRace.RaceCategory);
            Assert.Equal(originalRace.RaceDescription, editedRace.RaceDescription);
            Assert.Equal(originalRace.RaceHistory, editedRace.RaceHistory);
            Assert.Equal(originalRace.Conviction, editedRace.Conviction);
            Assert.Equal(originalRace.ZSM, editedRace.ZSM);
            Assert.Equal(originalRace.DomesticationValue, editedRace.DomesticationValue);
            Assert.Equal(originalRace.BaseInitiative, editedRace.BaseInitiative);
            Assert.Equal(originalRace.BaseXP, editedRace.BaseXP);
            Assert.Equal(originalRace.FightingSpiritNumber, editedRace.FightingSpiritNumber);
        }

        /// <summary>
        /// Ověří, že FromRace kopíruje statistiky.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_CopiesStats()
        {
            // Arrange
            var stats = new Dictionary<BodyStat, ValueRange>
            {
                { BodyStat.Intelligence, new ValueRange(10, 2, DiceType.D4) },
                { BodyStat.Charisma, new ValueRange(12, 2, DiceType.D6) }
            };

            var originalRace = new RaceBuilder()
                .WithName("Mág")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.B))
                .AddStat(stats)
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace).Build();

            // Assert
            Assert.NotNull(editedRace.StatsPrimar);
            Assert.Equal(2, editedRace.StatsPrimar.Count);
            Assert.True(editedRace.StatsPrimar.ContainsKey(BodyStat.Intelligence));
            Assert.True(editedRace.StatsPrimar.ContainsKey(BodyStat.Charisma));
        }

        /// <summary>
        /// Ověří, že FromRace kopíruje tělesné části.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_CopiesBodyParts()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Troll")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.D))
                .AddBodyPart(new BodyPart("hlava", BodyPartType.Head, 1))
                .AddBodyPart(new BodyPart("paže", BodyPartType.Arm, 2))
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace).Build();

            // Assert
            Assert.NotNull(editedRace.BodyParts);
            Assert.Equal(2, editedRace.BodyParts.Count);
        }

        /// <summary>
        /// Ověří, že FromRace kopíruje speciální schopnosti.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_CopiesSpecialAbilities()
        {
            // Arrange
            var ability1 = new SpecialAbilities("Nočné vidění", "Vidění v temnu");
            var ability2 = new SpecialAbilities("Regenerace", "Zotavení ze zranění");

            var originalRace = new RaceBuilder()
                .WithName("Drak")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.E))
                .AddSpecialAbility(ability1)
                .AddSpecialAbility(ability2)
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace).Build();

            // Assert
            Assert.NotNull(editedRace.SpecialAbilities);
            Assert.Equal(2, editedRace.SpecialAbilities.Count);
        }

        /// <summary>
        /// Ověří, že FromRace kopíruje hierarchii.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_CopiesHierarchy()
        {
            // Arrange
            var hierarchies = new List<string> { "Matka", "Dělnice", "Strážci" };

            var originalRace = new RaceBuilder()
                .WithName("Včelí rod")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.A0))
                .AddRaceHierarchy(hierarchies)
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace).Build();

            // Assert
            Assert.NotNull(editedRace.RaceHierarchySystem);
            Assert.Equal(3, editedRace.RaceHierarchySystem.Count);
        }

        #endregion

        #region Editace s fluent API - Scenario 3

        /// <summary>
        /// Ověří, že lze editovat popis existující rasy.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_EditDescription()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Elf")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.B))
                .WithDescription("Starý popis", "Stará historie")
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .WithDescription("Nový popis", "Nová historie")
                .Build();

            // Assert
            Assert.Same(originalRace, editedRace);
            Assert.Equal("Nový popis", editedRace.RaceDescription);
            Assert.Equal("Nová historie", editedRace.RaceHistory);
        }

        /// <summary>
        /// Ověří, že lze editovat iniciativu existující rasy.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_EditInitiative()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Skřet")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.C))
                .WithBaseInitiative(2)
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .WithBaseInitiative(4)
                .Build();

            // Assert
            Assert.Equal(4, editedRace.BaseInitiative);
        }

        /// <summary>
        /// Ověří, že lze přidat nové tělesné části existující rase.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_AddBodyParts()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Drak")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.E))
                .AddBodyPart(new BodyPart("hlava", BodyPartType.Head, 1))
                .Build();

            var originalCount = originalRace.BodyParts?.Count ?? 0;

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .AddBodyPart(new BodyPart("křídla", BodyPartType.Wing, 2))
                .AddBodyPart(new BodyPart("ocas", BodyPartType.Tail, 1))
                .Build();

            // Assert
            Assert.Equal(originalCount + 2, editedRace.BodyParts.Count);
        }

        /// <summary>
        /// Ověří, že lze přidat nové speciální schopnosti existující rase.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_AddSpecialAbilities()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Kouzelník")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.B))
                .AddSpecialAbility(new SpecialAbilities("Magická otpornost", "Odolnost vůči magii"))
                .Build();

            var originalCount = originalRace.SpecialAbilities?.Count ?? 0;

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .AddSpecialAbility(new SpecialAbilities("Teleportace", "Schopnost se teleportovat"))
                .Build();

            // Assert
            Assert.Equal(originalCount + 1, editedRace.SpecialAbilities.Count);
        }

        /// <summary>
        /// Ověří komplexní editaci existující rasy s více změnami.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_ComplexEdit()
        {
            // Arrange
            var originalRace = new RaceBuilder()
                .WithName("Goblin")
                .WithCategory(RaceCategory.Humanoid)
                .WithBodyDimensions(new BodyDimension(RaceSize.A))
                .WithDescription("Malí tvorové", "Tmavý původ")
                .WithBaseInitiative(3)
                .WithBaseXP(50)
                .AddBodyPart(new BodyPart("paže", BodyPartType.Arm, 2))
                .Build();

            // Act
            var editedRace = RaceBuilder.FromRace(originalRace)
                .WithDescription("Upravení goblini", "Nový začátek")
                .WithBaseInitiative(5)
                .WithBaseXP(100)
                .WithConviction(ConvictionType.Neutral)
                .AddBodyPart(new BodyPart("drápy", BodyPartType.Claw, 2))
                .AddSpecialAbility(new SpecialAbilities("Nočné vidění", "Vidění v noci"))
                .Build();

            // Assert
            Assert.Same(originalRace, editedRace);
            Assert.Equal("Upravení goblini", editedRace.RaceDescription);
            Assert.Equal("Nový začátek", editedRace.RaceHistory);
            Assert.Equal(5, editedRace.BaseInitiative);
            Assert.Equal(100, editedRace.BaseXP);
            Assert.Equal(ConvictionType.Neutral, editedRace.Conviction);
            Assert.Equal(2, editedRace.BodyParts.Count);
            Assert.Equal(1, editedRace.SpecialAbilities.Count);
        }

        #endregion

        #region Validace - Scenario 4

        /// <summary>
        /// Ověří, že FromRace vyhodí výjimku při null rase.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_FromRace_Null_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RaceBuilder.FromRace(null));
        }

        /// <summary>
        /// Ověří, že Build() vyhodí výjimku, když chybí jméno rasy.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_Build_MissingName_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new RaceBuilder().Build());
        }

        /// <summary>
        /// Ověří, že WithName vyhodí výjimku při prázdném jménu.
        /// </summary>
        [Fact]
        public void Test_RaceBuilder_WithName_Empty_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new RaceBuilder().WithName(""));
        }

        #endregion
    }
}
