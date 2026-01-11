using AutoMapper;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.InfrastructureTest.ArrangeData;
using Microsoft.Extensions.Logging;
using Xunit;

namespace HoL.InfrastructureTest.Mapping
{
    /// <summary>
    /// Testovací sada pro ověření mapování mezi RaceDbModel a Race entitami.
    /// Používá ArrangeData pro testovací data zajišťující konzistenci.
    /// </summary>
    public class RaceDbModelToRaceMappingTest
    {
        private readonly IMapper _mapper;

        public RaceDbModelToRaceMappingTest()
        {
            // Inicializace AutoMapperu s DomainInfrastructureMapper profilem
            var loggerFactory = new LoggerFactory();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainInfrastructureMapper>(), loggerFactory);
            _mapper = config.CreateMapper();
        }


        #region Mapování RaceDbModel → Race

        /// <summary>
        /// Ověří mapování základních vlastností RaceDbModel na Race - Elf.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsBasicProperties()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race);
            Assert.Equal("Elf", race.RaceName);
            Assert.Equal(RaceCategory.Humanoid, race.RaceCategory);
            Assert.Equal("Graceful and agile beings of nature", race.RaceDescription);
            Assert.Equal("Elves are ancient beings with long history", race.RaceHistory);
            Assert.Equal(ConvictionType.Good, race.Conviction);
            Assert.Equal(15, race.BaseInitiative);
            Assert.Equal(100, race.BaseXP);
            Assert.Equal(10, race.FightingSpiritNumber);
            Assert.Equal(12, race.ZSM);
            Assert.Equal(5, race.DomesticationValue);
        }

        /// <summary>
        /// Ověří mapování tělesných rozměrů (BodyDimension).
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsBodyDimensions()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.BodyDimensins);
            Assert.Equal(50, race.BodyDimensins.WeightMin);
            Assert.Equal(80, race.BodyDimensins.WeightMax);
            Assert.Equal(160, race.BodyDimensins.HeihtMin);
            Assert.Equal(190, race.BodyDimensins.HeihtMax);
            Assert.Equal(160, race.BodyDimensins.LengthMin);
            Assert.Equal(190, race.BodyDimensins.LengthMax);
        }

        /// <summary>
        /// Ověří mapování JSON serializovaných statistik na ValueRange objekty.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsStatistics()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.StatsPrimar);
            Assert.NotEmpty(race.StatsPrimar);
            Assert.True(race.StatsPrimar.ContainsKey(BodyStat.Strength));
            Assert.True(race.StatsPrimar.ContainsKey(BodyStat.Agility));
            
            // Ověření Strength
            var strength = race.StatsPrimar[BodyStat.Strength];
            Assert.Equal(5, strength.Min);
            Assert.Equal(3, strength.DiceCount);
            Assert.Equal(DiceType.D6, strength.DiceType);
            
            // Ověření Agility
            var agility = race.StatsPrimar[BodyStat.Agility];
            Assert.Equal(10, agility.Min);
            Assert.Equal(1, agility.DiceCount);
            Assert.Equal(DiceType.D6, agility.DiceType);
        }

        /// <summary>
        /// Ověří mapování JSON serializovaných zranitelností.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsVulnerabilities()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.Vulnerabilities);
            Assert.NotEmpty(race.Vulnerabilities);
            Assert.True(race.Vulnerabilities.ContainsKey(VulnerabilityType.PsychicAttack));
            Assert.True(race.Vulnerabilities.ContainsKey(VulnerabilityType.BluntForce));
            Assert.Equal(1.0, race.Vulnerabilities[VulnerabilityType.PsychicAttack]);
            Assert.Equal(0.5, race.Vulnerabilities[VulnerabilityType.BluntForce]);
        }

        /// <summary>
        /// Ověří mapování JSON serializované mobility.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsMobility()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.Mobility);
            Assert.NotEmpty(race.Mobility);
            Assert.True(race.Mobility.ContainsKey(MobilityType.Running));
            Assert.True(race.Mobility.ContainsKey(MobilityType.Swim));
            Assert.Equal(10, race.Mobility[MobilityType.Running]);
            Assert.Equal(2, race.Mobility[MobilityType.Swim]);
        }

        /// <summary>
        /// Ověří mapování JSON serializované hierarchie rasy.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsHierarchy()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.RaceHierarchySystem);
            Assert.NotEmpty(race.RaceHierarchySystem);
            Assert.Equal(3, race.RaceHierarchySystem.Count);
            Assert.Contains("Mudrc", race.RaceHierarchySystem);
            Assert.Contains("Vladce", race.RaceHierarchySystem);
            Assert.Contains("Rytir", race.RaceHierarchySystem);
        }

        /// <summary>
        /// Ověří mapování JSON serializovaných speciálních schopností.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_MapsSpecialAbilities()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            RaceDbModel elfDbModel = raceDbModels[0];

            // Act
            //var race = elfDbModel.MapToRace();
            var race = _mapper.Map<Race>(elfDbModel);

            // Assert
            Assert.NotNull(race.SpecialAbilities);
            Assert.NotEmpty(race.SpecialAbilities);
            Assert.Single(race.SpecialAbilities);
            var ability = race.SpecialAbilities[0];
            Assert.Equal("Dlouhovekost", ability.AbilityName);
            Assert.Equal("Ziji extremne dlouho", ability.AbilityDescription);
        }

        /// <summary>
        /// Ověří mapování Red Dragon - s prázdnými kolekcemi.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Dragon_MapsCompleteData()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var dragonDbModel = raceDbModels[1];

            // Act
            var race = dragonDbModel.MapToRace();

            // Assert
            Assert.NotNull(race);
            Assert.Equal("Red Dragon", race.RaceName);
            Assert.Equal(RaceCategory.Dragon, race.RaceCategory);
            Assert.Equal(ConvictionType.Evil, race.Conviction);
            Assert.Equal(20, race.BaseInitiative);
            Assert.Equal(500, race.BaseXP);
            Assert.Equal(25, race.FightingSpiritNumber);
            Assert.Equal(18, race.ZSM);
            Assert.Equal(0, race.DomesticationValue);
        }


        /// <summary>
        /// Ověří, že mapování tvoří Race instanci s Id = 1 pro Elfa.
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_Elf_PreservesId()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            RaceDbModel elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.Equal(1, race.Id);
        }

        #endregion

        #region Konzistence mapování

        /// <summary>
        /// Ověří, že RaceDbModel_Arrange a Race_Arrange mají konzistentní data.
        /// </summary>
        [Fact]
        public void Test_ArrangeData_RaceDbModel_And_Race_AreConsistent()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var races = ArrangeClass.Race_Arrange();
            var elfDbModel = raceDbModels[0];
            var elfRace = races[0];

            // Act
            var mappedRace = elfDbModel.MapToRace();

            // Assert - Ověříme konzistenci základních vlastností
            Assert.Equal(elfRace.RaceName, mappedRace.RaceName);
            Assert.Equal(elfRace.RaceCategory, mappedRace.RaceCategory);
            Assert.Equal(elfRace.RaceDescription, mappedRace.RaceDescription);
            Assert.Equal(elfRace.RaceHistory, mappedRace.RaceHistory);
            Assert.Equal(elfRace.Conviction, mappedRace.Conviction);
            Assert.Equal(elfRace.BaseInitiative, mappedRace.BaseInitiative);
            Assert.Equal(elfRace.BaseXP, mappedRace.BaseXP);
            Assert.Equal(elfRace.FightingSpiritNumber, mappedRace.FightingSpiritNumber);
            Assert.Equal(elfRace.ZSM, mappedRace.ZSM);
            Assert.Equal(elfRace.DomesticationValue, mappedRace.DomesticationValue);
        }

        /// <summary>
        /// Ověří, že všechny rasy z ArrangeData lze mapovat bez chyb.
        /// </summary>
        [Fact]
        public void Test_AllRaces_FromArrangeData_CanBeMapped()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();

            // Act & Assert
            foreach (var raceDbModel in raceDbModels)
            {
                var mappedRace = raceDbModel.MapToRace();
                Assert.NotNull(mappedRace);
                Assert.NotEmpty(mappedRace.RaceName);
            }
        }

        #endregion

        #region Mapování Treasure

        /// <summary>
        /// Ověří, že mapování správně mapuje přidružený poklad (Treasure).
        /// </summary>
        [Fact]
        public void Test_MapRaceDbModel_To_Race_MapsAttachedTreasure()
        {
            // Arrange
            var raceDbModels = ArrangeClass.RaceDbModel_Arrange();
            var elfDbModel = raceDbModels[0];

            // Act
            var race = elfDbModel.MapToRace();

            // Assert
            Assert.NotNull(race.Treasure);
            Assert.NotNull(race.Treasure.CurrencyGroup);
            Assert.Equal(1, race.Treasure.CurrencyGroup.Id);
        }

        #endregion
    }
}
