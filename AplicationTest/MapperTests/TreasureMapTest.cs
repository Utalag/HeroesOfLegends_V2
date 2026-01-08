using AplicationTest.MapperTest;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;

namespace AplicationTest.MapperTests
{
    public class TreasureMapTest : SetupMapperTests
    {
        // Domain -> DTO
        [Fact]
        public void Treasure_To_TreasureDto_Maps_CoinQuantities_And_CurrencyId()
        {
            // Arrange
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            var silver = new SingleCurrency("Silver", "st", 2, 10);
            var copper = new SingleCurrency("Copper", "md", 3, 100);
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup", new() { gold, silver, copper });

            var src = new Treasure(currencyGroup);
            src.AddCoins(1, 2);
            src.AddCoins(2, 5);
            src.AddCoins(3, 7);

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
            var gold = new SingleCurrency("Gold", "zl", 1, 1);
            var silver = new SingleCurrency("Silver", "st", 2, 10);
            var copper = new SingleCurrency("Copper", "md", 3, 100);
            var currencyGroup = new CurrencyGroup("Test CurrencyGroup", new() { gold, silver, copper });

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
            //Assert.Equal(1, domain.);
            //Assert.Equal(0, domain.GetAmount(2));
            //Assert.Equal(15, domain.GetAmount(3));
        }
    }
}
