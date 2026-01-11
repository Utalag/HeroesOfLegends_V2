using Xunit;
using HoL.InfrastructureTest.ArrangeData;

namespace HoL.InfrastructureTest
{
    /// <summary>
    /// Testy pro DbFixture - ověřuje, že jsou v databázi správné počty entit.
    /// </summary>
    public class DbFixtureTest
    {
        private readonly DbFixture _fixture;

        public DbFixtureTest()
        {
            _fixture = new DbFixture();
        }

        /// <summary>
        /// Ověří, že DbFixture inicializuje správný počet SingleCurrency entit (3x).
        /// </summary>
        [Fact]
        public void Seed_Should_Create_Three_SingleCurrencies()
        {
            // Act
            var singleCurrencies = _fixture.Context.SingleCurrencies.ToList();

            // Assert
            Assert.Equal(3, singleCurrencies.Count);
            Assert.Contains(singleCurrencies, sc => sc.Name == "Gold" && sc.Id == 1);
            Assert.Contains(singleCurrencies, sc => sc.Name == "Silver" && sc.Id == 2);
            Assert.Contains(singleCurrencies, sc => sc.Name == "Copper" && sc.Id == 3);
        }

        /// <summary>
        /// Ověří, že DbFixture inicializuje správný počet CurrencyGroup entit (1x).
        /// </summary>
        [Fact]
        public void Seed_Should_Create_One_CurrencyGroup()
        {
            // Act
            var currencyGroups = _fixture.Context.CurrencyGroups.ToList();

            // Assert
            Assert.Single(currencyGroups);
            Assert.Equal("FantasyCoins", currencyGroups[0].GroupName);
        }

        /// <summary>
        /// Ověří, že CurrencyGroup má správný počet SingleCurrency (3x).
        /// </summary>
        [Fact]
        public void Seed_Should_Create_CurrencyGroup_With_Three_Currencies()
        {
            // Act
            var currencyGroup = _fixture.Context.CurrencyGroups.FirstOrDefault();

            // Assert
            Assert.NotNull(currencyGroup);
            Assert.Equal(3, currencyGroup.Currencies.Count);
        }

        /// <summary>
        /// Ověří, že DbFixture inicializuje správný počet Race entit (2x).
        /// </summary>
        [Fact]
        public void Seed_Should_Create_Two_Races()
        {
            // Act
            var races = _fixture.Context.Races.ToList();

            // Assert
            Assert.Equal(2, races.Count);
            Assert.Contains(races, r => r.RaceName == "Elf" && r.Id == 1);
            Assert.Contains(races, r => r.RaceName == "Red Dragon" && r.Id == 2);
        }

        /// <summary>
        /// Ověří, že každá Race má přiřazeno Treasure (2x).
        /// </summary>
        [Fact]
        public void Seed_Should_Create_Two_Treasures_One_Per_Race()
        {
            // Act
            var races = _fixture.Context.Races.ToList();

            // Assert
            Assert.Equal(2, races.Count);
            
            // Oba Races mají Treasure
            foreach (var race in races)
            {
                Assert.NotNull(race.Treasure);
                Assert.Equal(1, race.Treasure.CurrencyId); // Obě odkazují na CurrencyGroup ID = 1
            }
        }

        /// <summary>
        /// Ověří kompletní stav databáze - všechny entity na místě.
        /// </summary>
        [Fact]
        public void Seed_Should_Create_All_Entities_Correctly()
        {
            // Act
            var singleCurrencies = _fixture.Context.SingleCurrencies.Count();
            var currencyGroups = _fixture.Context.CurrencyGroups.Count();
            var races = _fixture.Context.Races.Count();

            // Assert
            Assert.Equal(3, singleCurrencies); // 3x SingleCurrency (Gold, Silver, Copper)
            Assert.Equal(1, currencyGroups);   // 1x CurrencyGroup (FantasyCoins)
            Assert.Equal(2, races);             // 2x Race (Elf, Red Dragon)
        }
    }
}
