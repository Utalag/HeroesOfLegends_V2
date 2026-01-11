using Xunit;
using HoL.Infrastructure.Data;
using HoL.InfrastructureTest.ArrangeData;

namespace HoL.InfrastructureTest
{
    /// <summary>
    /// Testy pro DbInMemoryFactory - ověřuje správné vytváření In-Memory databází.
    /// </summary>
    public class DbInMemoryFactoryTest
    {
        /// <summary>
        /// Ověří, že CreateDbContext bez parametru vytvoří novou, jedinečnou databázi.
        /// </summary>
        [Fact]
        public void CreateDbContext_WithoutParameter_Creates_Unique_Database()
        {
            // Act
            var context1 = DbInMemoryFactory.CreateDbContext();
            var context2 = DbInMemoryFactory.CreateDbContext();

            // Assert - obě databáze jsou nezávislé
            context1.SingleCurrencies.Add(ArrangeClass.SingleCurrencyDbModel_Arrange()[0]);
            context1.SaveChanges();

            // context2 by neměl obsahovat data z context1
            Assert.Single(context1.SingleCurrencies);
            Assert.Empty(context2.SingleCurrencies);
        }

        /// <summary>
        /// Ověří, že CreateDbContext se stejným jménem sdílí data.
        /// </summary>
        [Fact]
        public void CreateDbContext_WithSameName_Shares_Data()
        {
            // Arrange
            const string dbName = "SharedTestDb";

            // Act
            var context1 = DbInMemoryFactory.CreateDbContext(dbName);
            var context2 = DbInMemoryFactory.CreateDbContextForExistingDatabase(dbName);

            // Assert - obě databáze sdílí data
            var currency = ArrangeClass.SingleCurrencyDbModel_Arrange()[0];
            context1.SingleCurrencies.Add(currency);
            context1.SaveChanges();

            // context2 by měl vidět data z context1
            Assert.Single(context1.SingleCurrencies);
            Assert.Single(context2.SingleCurrencies);
        }

        /// <summary>
        /// Ověří, že CreateDbContext s null parametrem vytvoří novou GUID databázi.
        /// </summary>
        [Fact]
        public void CreateDbContext_WithNull_Creates_New_Guid_Database()
        {
            // Act
            var context1 = DbInMemoryFactory.CreateDbContext(null);
            var context2 = DbInMemoryFactory.CreateDbContext(null);

            // Assert - obě jsou nezávislé
            context1.SingleCurrencies.Add(ArrangeClass.SingleCurrencyDbModel_Arrange()[0]);
            context1.SaveChanges();

            Assert.Single(context1.SingleCurrencies);
            Assert.Empty(context2.SingleCurrencies);
        }

        /// <summary>
        /// Ověří, že CreateDbContextForExistingDatabase vyhodí výjimku s prázdným jménem.
        /// </summary>
        [Fact]
        public void CreateDbContextForExistingDatabase_WithEmpty_Throws_Exception()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                DbInMemoryFactory.CreateDbContextForExistingDatabase(""));
        }

        /// <summary>
        /// Ověří, že CreateDbContext vrátí zdravý SqlDbContext instance.
        /// </summary>
        [Fact]
        public void CreateDbContext_Returns_Valid_SqlDbContext()
        {
            // Act
            var context = DbInMemoryFactory.CreateDbContext("ValidDbTest");

            // Assert
            Assert.NotNull(context);
            Assert.NotNull(context.SingleCurrencies);
            Assert.NotNull(context.CurrencyGroups);
            Assert.NotNull(context.Races);
        }
    }
}
