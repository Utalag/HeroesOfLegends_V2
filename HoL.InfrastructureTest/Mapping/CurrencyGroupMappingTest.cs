using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.InfrastructureTest.ArrangeData;
using Xunit;

namespace HoL.InfrastructureTest.Mapping
{
    /// <summary>
    /// Testovací sada pro ověření mapování mezi CurrencyGroupDbModel a CurrencyGroup entitami.
    /// Používá ArrangeData pro testovací data zajišťující konzistenci.
    /// </summary>
    public class CurrencyGroupMappingTest
    {
        #region Mapování CurrencyGroupDbModel → CurrencyGroup

        /// <summary>
        /// Ověří mapování základních vlastností CurrencyGroupDbModel na CurrencyGroup.
        /// </summary>
        [Fact]
        public void Test_MapCurrencyGroupDbModel_To_CurrencyGroup_MapsBasicProperties()
        {
            // Arrange
            var currencyGroupDbModel = ArrangeClass.CurrencyGroupDbModel_Arrange();
            var expectedCurrencies = ArrangeClass.SingleCurrencyDbModel_Arrange();

            // Act
            var currencyGroup = currencyGroupDbModel.MapToCurrencyGroup();

            // Assert
            Assert.NotNull(currencyGroup);
            Assert.NotNull(currencyGroup.Currencies);
            Assert.Equal(3, currencyGroup.Currencies.Count);
        }

        /// <summary>
        /// Ověří, že mapování správně mapuje jednotlivé měny.
        /// </summary>
        [Fact]
        public void Test_MapCurrencyGroupDbModel_To_CurrencyGroup_MapsCurrencies()
        {
            // Arrange
            var currencyGroupDbModel = ArrangeClass.CurrencyGroupDbModel_Arrange();

            // Act
            var currencyGroup = currencyGroupDbModel.MapToCurrencyGroup();

            // Assert
            Assert.NotNull(currencyGroup.Currencies);
            Assert.Equal(3, currencyGroup.Currencies.Count);

            // Ověření jednotlivých měn
            var gold = currencyGroup.Currencies[0];
            Assert.Equal("Gold", gold.Name);
            Assert.Equal("gl", gold.ShotName);
            Assert.Equal(1, gold.ExchangeRate);

            var silver = currencyGroup.Currencies[1];
            Assert.Equal("Silver", silver.Name);
            Assert.Equal("sl", silver.ShotName);
            Assert.Equal(10, silver.ExchangeRate);

            var copper = currencyGroup.Currencies[2];
            Assert.Equal("Copper", copper.Name);
            Assert.Equal("cp", copper.ShotName);
            Assert.Equal(100, copper.ExchangeRate);
        }

        /// <summary>
        /// Ověří, že mapování zachovává pořadí měn.
        /// </summary>
        [Fact]
        public void Test_MapCurrencyGroupDbModel_To_CurrencyGroup_PreservesCurrencyOrder()
        {
            // Arrange
            var currencyGroupDbModel = ArrangeClass.CurrencyGroupDbModel_Arrange();

            // Act
            var currencyGroup = currencyGroupDbModel.MapToCurrencyGroup();

            // Assert
            Assert.Equal("Gold", currencyGroup.Currencies[0].Name);
            Assert.Equal("Silver", currencyGroup.Currencies[1].Name);
            Assert.Equal("Copper", currencyGroup.Currencies[2].Name);
        }

        #endregion

        #region Konzistence mapování

        /// <summary>
        /// Ověří, že CurrencyGroupDbModel_Arrange a CurrencyGroup_Arrange mají konzistentní data.
        /// </summary>
        [Fact]
        public void Test_ArrangeData_CurrencyGroupDbModel_And_CurrencyGroup_AreConsistent()
        {
            // Arrange
            var currencyGroupDbModel = ArrangeClass.CurrencyGroupDbModel_Arrange();
            var currencyGroupDomain = ArrangeClass.CurrencyGroup_Arrange();

            // Act
            var mappedCurrencyGroup = currencyGroupDbModel.MapToCurrencyGroup();

            // Assert
            Assert.Equal(currencyGroupDomain.Currencies.Count, mappedCurrencyGroup.Currencies.Count);
            for (int i = 0; i < currencyGroupDomain.Currencies.Count; i++)
            {
                Assert.Equal(currencyGroupDomain.Currencies[i].Name, mappedCurrencyGroup.Currencies[i].Name);
                Assert.Equal(currencyGroupDomain.Currencies[i].ExchangeRate, mappedCurrencyGroup.Currencies[i].ExchangeRate);
            }
        }

        #endregion
    }
}
