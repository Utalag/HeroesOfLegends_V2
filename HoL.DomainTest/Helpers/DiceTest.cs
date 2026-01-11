using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using Xunit;

namespace HoL.DomainTest.Helpers
{
    public class DiceTest
    {
        [Fact]
        public void Dice_CreateWithParameter()
        {
            // Arrange & Act
            var dice = new Dice(2, HoL.Domain.Enums.DiceType.D8, 3);

            // Assert
            Assert.Equal(2, dice.Count);
            Assert.Equal(DiceType.D8, dice.Sides);
            Assert.Equal(3, dice.Bonus);
        }

        [Fact]
        public void Dice_WihoutParamater_DefaultValues()
        {
            // Arrange & Act
            var dice = new Dice();
            // Assert
            Assert.Equal(1, dice.Count);
            Assert.Equal(DiceType.D6, dice.Sides);
            Assert.Equal(0, dice.Bonus);
        }

        [Fact]
        public void Dice_SetQuantity_ValidValue_UpdatesCount()
        {
            // Arrange
            var dice = new Dice();
            // Act
            dice.SetQuantity(5);
            // Assert
            Assert.Equal(5, dice.Count);
        }

        [Fact]
        public void Dice_SetQuantity_InvalidValue_ThrowsException()
        {
            // Arrange
            var dice = new Dice();
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => dice.SetQuantity(0));
        }

        [Fact]
        public void Dice_FluentApi()
        {
            // Arrange 
            var dice = new Dice();

            // Act
            dice.SetQuantity(4)
                .SetBonus(2)
                .SetSides(DiceType.D10);

            // Assert
            Assert.Equal(4, dice.Count);
            Assert.Equal(DiceType.D10, dice.Sides);
            Assert.Equal(2, dice.Bonus);
        }
    }
}
