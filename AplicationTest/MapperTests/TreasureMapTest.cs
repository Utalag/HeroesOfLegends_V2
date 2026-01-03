using System.Collections.Generic;
using AplicationTest.MapperTest;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Domain.Helpers;
using HoL.Domain.ValueObjects;
using Xunit;

namespace AplicationTest.MapperTests
{
    public class TreasureMapTest : SetupMapperTests
    {
        // Domain -> DTO
        [Fact]
        public void Treasure_To_TreasureDto_Maps_CoinQuantities_And_CurrencyId()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            currencyGroup.Add(new SingleCurrency("Gold", "zl", 1, 1));
            currencyGroup.Add(new SingleCurrency("Silver", "st", 2, 10));
            currencyGroup.Add(new SingleCurrency("Copper", "md", 3, 100));

            var src = new Treasure(currencyGroup);
            src.Add(1, 2);
            src.Add(2, 5);
            src.Add(3, 7);

            // Act
            var dto = _mapper.Map<TreasureDto>(src);

            // Assert
            Assert.Equal(currencyGroup.Id, dto.CurrencySetId);
            Assert.Equal(2, dto.Amounts[1]);
            Assert.Equal(5, dto.Amounts[2]);
            Assert.Equal(7, dto.Amounts[3]);
        }

        // DTO -> Domain
        [Fact]
        public void TreasureDto_To_Treasure_Maps_CoinQuantities_And_CurrencyId()
        {
            // Arrange
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup");
            currencyGroup.Add(new SingleCurrency("Gold", "zl", 1, 1));
            currencyGroup.Add(new SingleCurrency("Silver", "st", 2, 10));
            currencyGroup.Add(new SingleCurrency("Copper", "md", 3, 100));

            var dto = new TreasureDto
            {
                CurrencySetId = currencyGroup.Id,
                Amounts = new Dictionary<int, int>
                {
                    [1] = 1,
                    [2] = 0,
                    [3] = 15,
                }
            };

            // Act
            var domain = _mapper.Map<Treasure>(dto);

            // Assert
            Assert.Equal(currencyGroup.Id, domain.CurrencyGroupId);
            Assert.Equal(1, domain.GetAmount(1));
            Assert.Equal(0, domain.GetAmount(2));
            Assert.Equal(15, domain.GetAmount(3));
        }
    }
}
