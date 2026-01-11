using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.InfrastructureTest.ArrangeData;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace HoL.InfrastructureTest.Mapping
{
    /// <summary>
    /// Testovací sada pro ověření mapování mezi TreasureDbModel a Treasure entitami.
    /// Používá ArrangeData pro testovací data zajišťující konzistenci.
    /// </summary>
    public class TreasureMappingTest
    {
        private readonly IMapper _mapper;

        public TreasureMappingTest()
        {
            // Inicializace AutoMapperu s DomainInfrastructureMapper profilem
            var loggerFactory = new LoggerFactory();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainInfrastructureMapper>(), loggerFactory);
            _mapper = config.CreateMapper();
        }

        #region Mapování TreasureDbModel → Treasure (Extension metody)

        /// <summary>
        /// Ověří mapování základních vlastností TreasureDbModel na Treasure.
        /// </summary>
        [Fact]
        public void Test_MapTreasureDbModel_To_Treasure_MapsBasicProperties()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();

            // Act
            var treasure = treasureDbModel.MapToTreasure();

            // Assert
            Assert.NotNull(treasure);
            Assert.NotNull(treasure.CurrencyGroup);
            Assert.Equal(1, treasure.CurrencyGroup.Id);
        }

        /// <summary>
        /// Ověří, že mapování správně mapuje měnovou skupinu.
        /// </summary>
        [Fact]
        public void Test_MapTreasureDbModel_To_Treasure_MapsCurrencyGroup()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();

            // Act
            var treasure = treasureDbModel.MapToTreasure();

            // Assert
            Assert.NotNull(treasure.CurrencyGroup);
            Assert.Equal("FantasyCoins", treasure.CurrencyGroup.GroupName);
            Assert.Equal(3, treasure.CurrencyGroup.Currencies.Count);
        }

        /// <summary>
        /// Ověří, že mapování správně deserializuje JSON s mincemi.
        /// </summary>
        [Fact]
        public void Test_MapTreasureDbModel_To_Treasure_DeserializesCoinQuantities()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();

            // Act
            var treasure = treasureDbModel.MapToTreasure();

            // Assert
            Assert.NotNull(treasure.CoinQuantities);
            Assert.NotEmpty(treasure.CoinQuantities);
            // Ověření, že koiny jsou správně načteny
            Assert.Equal(100, treasure.CoinQuantities[1]); // 100 Gold
            Assert.Equal(50, treasure.CoinQuantities[2]);  // 50 Silver
            Assert.Equal(25, treasure.CoinQuantities[3]);  // 25 Copper
        }

        /// <summary>
        /// Ověří, že mapování zachovává ID měnové skupiny.
        /// </summary>
        [Fact]
        public void Test_MapTreasureDbModel_To_Treasure_PreservesCurrencyGroupId()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();

            // Act
            var treasure = treasureDbModel.MapToTreasure();

            // Assert
            Assert.Equal(1, treasure.CurrencyGroup.Id);
        }

        #endregion

        #region Mapování přes AutoMapper (Treasure ↔ TreasureDbModel)

        /// <summary>
        /// Ověří mapování Domain modelu Treasure na Database model TreasureDbModel přes AutoMapper.
        /// </summary>
        [Fact]
        public void Test_AutoMapper_Maps_Treasure_To_TreasureDbModel()
        {
            // Arrange
            var treasure = ArrangeClass.Treasure_Arrange();

            // Act
            var treasureDbModel = _mapper.Map<TreasureDbModel>(treasure);

            // Assert
            Assert.NotNull(treasureDbModel);
            Assert.Equal(treasure.CurrencyGroupId, treasureDbModel.CurrencyId);
            Assert.NotNull(treasureDbModel.CoinQuantitiesJson);
            Assert.NotEmpty(treasureDbModel.CoinQuantitiesJson);
        }

        /// <summary>
        /// Ověří mapování Database modelu TreasureDbModel na Domain model Treasure přes AutoMapper.
        /// </summary>
        [Fact]
        public void Test_AutoMapper_Maps_TreasureDbModel_To_Treasure()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();

            // Act
            var treasure = _mapper.Map<Treasure>(treasureDbModel);

            // Assert
            Assert.NotNull(treasure);
            Assert.NotNull(treasure.CurrencyGroup);
            Assert.Equal("FantasyCoins", treasure.CurrencyGroup.GroupName);
            Assert.Equal(3, treasure.CurrencyGroup.Currencies.Count);
        }

        /// <summary>
        /// Round-trip test: Treasure → TreasureDbModel → Treasure
        /// Ověří, že hodnoty zůstávají identické po mapování tam a zpět.
        /// </summary>
        [Fact]
        public void Test_AutoMapper_RoundTrip_Treasure_To_TreasureDbModel_To_Treasure()
        {
            // Arrange
            var originalTreasure = ArrangeClass.Treasure_Arrange();

            // Act - Mapování tam a zpět
            var treasureDbModel = _mapper.Map<TreasureDbModel>(originalTreasure);
            var mappedTreasure = _mapper.Map<Treasure>(treasureDbModel);

            // Assert - Ověření totožných hodnot
            Assert.NotNull(mappedTreasure);
            Assert.NotNull(mappedTreasure.CurrencyGroup);

            // Ověření CurrencyGroup
            Assert.Equal(originalTreasure.CurrencyGroup.Id, mappedTreasure.CurrencyGroup.Id);
            Assert.Equal(originalTreasure.CurrencyGroup.GroupName, mappedTreasure.CurrencyGroup.GroupName);
            Assert.Equal(originalTreasure.CurrencyGroup.Currencies.Count, mappedTreasure.CurrencyGroup.Currencies.Count);

            // Ověření CurrencyGroupId
            Assert.Equal(originalTreasure.CurrencyGroupId, mappedTreasure.CurrencyGroupId);

            // Ověření CoinQuantities
            Assert.NotNull(mappedTreasure.CoinQuantities);
            Assert.Equal(originalTreasure.CoinQuantities.Count, mappedTreasure.CoinQuantities.Count);

            foreach (var kvp in originalTreasure.CoinQuantities)
            {
                Assert.True(mappedTreasure.CoinQuantities.ContainsKey(kvp.Key),
                    $"Key {kvp.Key} chybí v mapovaném pokladu");
                Assert.Equal(kvp.Value, mappedTreasure.CoinQuantities[kvp.Key]);
            }
        }

        /// <summary>
        /// Ověří, že CoinQuantities jsou správně zachovány při round-trip mapování.
        /// </summary>
        [Fact]
        public void Test_AutoMapper_RoundTrip_PreservesCoinQuantities()
        {
            // Arrange
            var originalTreasure = ArrangeClass.Treasure_Arrange();
            var expectedGoldQuantity = originalTreasure.CoinQuantities[1]; // 100 Gold
            var expectedSilverQuantity = originalTreasure.CoinQuantities[2]; // 50 Silver
            var expectedCopperQuantity = originalTreasure.CoinQuantities[3]; // 25 Copper

            // Act
            var treasureDbModel = _mapper.Map<TreasureDbModel>(originalTreasure);
            var mappedTreasure = _mapper.Map<Treasure>(treasureDbModel);

            // Assert - Ověření konkrétních hodnot
            Assert.Equal(expectedGoldQuantity, mappedTreasure.CoinQuantities[1]);
            Assert.Equal(expectedSilverQuantity, mappedTreasure.CoinQuantities[2]);
            Assert.Equal(expectedCopperQuantity, mappedTreasure.CoinQuantities[3]);
        }

        #endregion

        #region Konzistence mapování

        /// <summary>
        /// Ověří, že TreasureDbModel_Arrange a Treasure_Arrange mají konzistentní data.
        /// </summary>
        [Fact]
        public void Test_ArrangeData_TreasureDbModel_And_Treasure_AreConsistent()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();
            var treasureDomain = ArrangeClass.Treasure_Arrange();

            // Act
            var mappedTreasure = treasureDbModel.MapToTreasure();

            // Assert - Ověříme konzistenci základních vlastností
            Assert.NotNull(mappedTreasure.CurrencyGroup);
            Assert.NotNull(treasureDomain.CurrencyGroup);
            Assert.Equal(treasureDomain.CurrencyGroup.GroupName, mappedTreasure.CurrencyGroup.GroupName);
            Assert.Equal(treasureDomain.CurrencyGroup.Currencies.Count, mappedTreasure.CurrencyGroup.Currencies.Count);
        }

        /// <summary>
        /// Ověří, že mapování vytváří stejné množství mincí jako Domain model.
        /// </summary>
        [Fact]
        public void Test_ArrangeData_Treasure_CoinQuantities_AreConsistent()
        {
            // Arrange
            var treasureDbModel = ArrangeClass.TreasureDbModel_Arrange();
            var treasureDomain = ArrangeClass.Treasure_Arrange();

            // Act
            var mappedTreasure = treasureDbModel.MapToTreasure();

            // Assert
            Assert.Equal(treasureDomain.CoinQuantities.Count, mappedTreasure.CoinQuantities.Count);
            foreach (var kvp in treasureDomain.CoinQuantities)
            {
                Assert.True(mappedTreasure.CoinQuantities.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, mappedTreasure.CoinQuantities[kvp.Key]);
            }
        }

        #endregion
    }
}
