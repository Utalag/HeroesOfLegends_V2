using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.Infrastructure.Repositories;
using HoL.InfrastructureTest.ArrangeData;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HoL.InfrastructureTest.Repository.SingleCurrencyRepo
{
    public class SingleCurrencyRepoTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        public SingleCurrencyRepoTest(DbFixture fixture)
        {
            _fixture = fixture;
        }

        #region Read Operations - GetByIdAsync

        /// <summary>
        /// Ověří, že GetByIdAsync vrátí správnou entitu pro existující ID.
        /// </summary>
        [Fact]
        public async Task GetByIdAsyncReturnCurrentEntity()
        {
            // Arrange
            var expectedCurrencies = ArrangeClass.SingleCurrency_Arrange();
            var expectedGold = expectedCurrencies[0]; // Gold s ID = 1
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGold.Id, result.Id);
            Assert.Equal(expectedGold.Name, result.Name);
            Assert.Equal(expectedGold.ShotName, result.ShotName);
            Assert.Equal(expectedGold.ExchangeRate, result.ExchangeRate);
            Assert.Equal(expectedGold.HierarchyLevel, result.HierarchyLevel);
        }

        /// <summary>
        /// Ověří, že GetByIdAsync vrátí null pro neexistující ID.
        /// </summary>
        [Fact]
        public async Task GetByIdAsyncReturnsNullForNonExistentId()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Ověří, že GetByIdAsync vrátí Silver pro ID = 2.
        /// </summary>
        [Fact]
        public async Task GetByIdAsyncReturnsSilverForId2()
        {
            // Arrange
            var expectedCurrencies = ArrangeClass.SingleCurrency_Arrange();
            var expectedSilver = expectedCurrencies[1]; // Silver s ID = 2
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSilver.Id, result.Id);
            Assert.Equal("Silver", result.Name);
            Assert.Equal(10, result.ExchangeRate);
        }

        #endregion

        #region Read Operations - GetByNameAsync

        /// <summary>
        /// Ověří, že GetByNameAsync vrátí správnou měnu podle jména.
        /// </summary>
        [Fact]
        public async Task GetByNameAsyncReturnsCorrectCurrency()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByNameAsync("Gold");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Gold", result.Name);
            Assert.Equal(1, result.ExchangeRate);
            Assert.Equal("gl", result.ShotName);
        }

        /// <summary>
        /// Ověří, že GetByNameAsync vrátí null pro neexistující jméno.
        /// </summary>
        [Fact]
        public async Task GetByNameAsyncReturnsNullForNonExistentName()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByNameAsync("NonExistent");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Read Operations - ListAsync

        /// <summary>
        /// Ověří, že ListAsync vrátí všechny měny.
        /// </summary>
        [Fact]
        public async Task ListAsyncReturnsAllCurrencies()
        {
            // Arrange
            var repository = CreateRepository();
            var expectedCount = 3; // Gold, Silver, Copper

            // Act
            var result = await repository.ListAsync();

            // Assert
            Assert.NotNull(result);
            var currencyList = result.ToList();
            Assert.Equal(expectedCount, currencyList.Count);
            Assert.Contains(currencyList, c => c.Name == "Gold");
            Assert.Contains(currencyList, c => c.Name == "Silver");
            Assert.Contains(currencyList, c => c.Name == "Copper");
        }

        #endregion

        #region Read Operations - ExistsAsync

        /// <summary>
        /// Ověří, že ExistsAsync vrátí true pro existující ID.
        /// </summary>
        [Fact]
        public async Task ExistsAsyncReturnsTrueForExistingId()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.ExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Ověří, že ExistsAsync vrátí false pro neexistující ID.
        /// </summary>
        [Fact]
        public async Task ExistsAsyncReturnsFalseForNonExistentId()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Read Operations - GetByIdsAsync

        /// <summary>
        /// Ověří, že GetByIdsAsync vrátí měny pro zadaná ID.
        /// </summary>
        [Fact]
        public async Task GetByIdsAsyncReturnsCorrectCurrencies()
        {
            // Arrange
            var repository = CreateRepository();
            var ids = new List<int> { 1, 2 };

            // Act
            var result = await repository.GetByIdsAsync(ids);

            // Assert
            Assert.NotNull(result);
            var currencyList = result.ToList();
            Assert.Equal(2, currencyList.Count);
            Assert.Contains(currencyList, c => c.Name == "Gold");
            Assert.Contains(currencyList, c => c.Name == "Silver");
            Assert.DoesNotContain(currencyList, c => c.Name == "Copper");
        }

        #endregion

        #region Read Operations - GetPageAsync

        /// <summary>
        /// Ověří, že GetPageAsync vrátí stránkovaný seznam měn.
        /// </summary>
        [Fact]
        public async Task GetPageAsyncReturnsFirstPage()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetPageAsync(page: 1, size: 2);

            // Assert
            Assert.NotNull(result);
            var currencyList = result.ToList();
            Assert.Equal(2, currencyList.Count);
        }

        /// <summary>
        /// Ověří, že GetPageAsync vrátí druhou stránku.
        /// </summary>
        [Fact]
        public async Task GetPageAsyncReturnsSecondPage()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetPageAsync(page: 2, size: 2);

            // Assert
            Assert.NotNull(result);
            var currencyList = result.ToList();
            Assert.Single(currencyList);
            Assert.Equal("Copper", currencyList[0].Name);
        }

        #endregion

        #region Write Operations - AddAsync

        /// <summary>
        /// Ověří, že AddAsync přidá novou měnu do databáze.
        /// </summary>
        [Fact]
        public async Task AddAsyncAddsNewCurrency()
        {
            // Arrange
            var repository = CreateRepository();
            var newCurrency = new SingleCurrency("Platinum", "pt", 4, 1000);
            var context = _fixture.Context;

            // Act
            var newId = await repository.AddAsync(newCurrency);

            // Assert
            Assert.True(newId > 0);
            var addedCurrency = await repository.GetByIdAsync(newId);
            Assert.NotNull(addedCurrency);
            Assert.Equal("Platinum", addedCurrency.Name);
            Assert.Equal("pt", addedCurrency.ShotName);
            Assert.Equal(1000, addedCurrency.ExchangeRate);
        }

        #endregion

        #region Write Operations - UpdateAsync

        /// <summary>
        /// Ověří, že UpdateAsync aktualizuje existující měnu.
        /// </summary>
        [Fact]
        public async Task UpdateAsyncUpdatesExistingCurrency()
        {
            // Arrange
            var repository = CreateRepository();
            var currencyToUpdate = await repository.GetByIdAsync(1);
            Assert.NotNull(currencyToUpdate);

            // Změní exchange rate
            var updatedCurrency = new SingleCurrency(
                currencyToUpdate.Name,
                currencyToUpdate.ShotName,
                currencyToUpdate.HierarchyLevel,
                2000  // Nový exchange rate
            );
            updatedCurrency.SetId(1);

            // Act
            await repository.UpdateAsync(updatedCurrency);

            // Assert
            var result = await repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(2000, result.ExchangeRate);
        }

        /// <summary>
        /// Ověří, že UpdateAsync vyhodí výjimku pro neexistující měnu.
        /// </summary>
        [Fact]
        public async Task UpdateAsyncThrowsExceptionForNonExistentCurrency()
        {
            // Arrange
            var repository = CreateRepository();
            var nonExistentCurrency = new SingleCurrency("NonExistent", "ne", 99, 100);
            nonExistentCurrency.SetId(999);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateAsync(nonExistentCurrency));
        }

        #endregion

        #region Write Operations - DeleteAsync

        /// <summary>
        /// Ověří, že DeleteAsync odstraní měnu z databáze.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncRemovesCurrency()
        {
            // Arrange
            var repository = CreateRepository();
            var currencyToDelete = await repository.GetByIdAsync(3);
            Assert.NotNull(currencyToDelete);

            // Act
            await repository.DeleteAsync(3);

            // Assert
            var result = await repository.GetByIdAsync(3);
            Assert.Null(result);
        }

        /// <summary>
        /// Ověří, že DeleteAsync se bezpečně vyrovná s neexistujícím ID.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncHandlesNonExistentId()
        {
            // Arrange
            var repository = CreateRepository();

            // Act & Assert - nemělo by vyhodit výjimku
            await repository.DeleteAsync(999);
        }

        #endregion

        #region Helper Methods

        private SingleCurrencyDbRepository CreateRepository()
        {
            var mockLogger = new Mock<ILogger>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();

            // Příprava mockovaného loggeru z factory
            mockLoggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(mockLogger.Object);

            // Použití skutečného mapperu s factory
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<DomainInfrastructureMapper>(), mockLoggerFactory.Object);
            var mapper = mapperConfig.CreateMapper();

            return new SingleCurrencyDbRepository(
                _fixture.Context,
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mapper
            );
        }

        #endregion
    }
}
