using HoL.Domain.Helpers;
using Xunit;

namespace HoL.DomainTest.Helpers
{
    public class StatBonusTest
    {
        [Theory]
        [InlineData(1,-5)]                                          //-5
        [InlineData(2,-4)]  [InlineData(3,-4)]                      //-4
        [InlineData(4,-3)]  [InlineData(5,-3)]                      //-3
        [InlineData(6,-2)]  [InlineData(7,-2)]                      //-2
        [InlineData(8,-1)]  [InlineData(9,-1)]                      //-1
        [InlineData(10,0)]  [InlineData(11,0)]  [InlineData(12,0)]  // 0
        [InlineData(13,1)]  [InlineData(14,1)]                      //+1
        [InlineData(15,2)]  [InlineData(16,2)]                      //+2
        [InlineData(17,3)]  [InlineData(18,3)]                      //+3    
        [InlineData(19,4)]  [InlineData(20,4)]                      //+4
        [InlineData(21,5)]  [InlineData(22,5)]                      //+5
        [InlineData(23,6)]  [InlineData(24,6)]                      //+6
        [InlineData(25,7)]  [InlineData(26,7)]                      //+7
        [InlineData(27,8)]  [InlineData(28,8)]                      //+8
        [InlineData(29,9)]  [InlineData(30,9)]                      //+9
        [InlineData(31,10)] [InlineData(32,10)]                     //+10
        [InlineData(33,11)] [InlineData(34,11)]                     //+11
        [InlineData(35,12)] [InlineData(36,12)]                     //+12
        [InlineData(37,13)] [InlineData(38,13)]                     //+13
        [InlineData(39,14)] [InlineData(40,14)]                     //+14
        [InlineData(41,15)] [InlineData(42,15)]                     //+15
        public void GetBonus_Valid(int input, int result)
        {
            Assert.Equal(result,StatBonusHelper.GetBonus(input));
        }

    }
}
