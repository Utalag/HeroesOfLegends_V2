using HoL.Domain.Entities.CurencyEntities;
using Xunit;

namespace HoL.DomainTest.ClassTested;

public class SingleCurrency_Test
{
    [Fact]
    public void Constructor_ValidParameters_InitializesProperties()
    {
        var currency = new SingleCurrency("Galeon", "gl", 1, 1);

        Assert.Equal(1, currency.HierarchyLevel);
        Assert.Equal("Galeon", currency.Name);
        Assert.Equal("gl", currency.ShotName);
        Assert.Equal(1, currency.ExchangeRate);
        Assert.Equal(0, currency.Id);
    }

    [Theory]
    [InlineData(null, "gl", 1, 1, "name")]
    [InlineData("Galeon", null, 1, 1, "shortName")]
    [InlineData("Galeon", "gl", 0, 1, "level")]
    [InlineData("Galeon", "gl", 1, 0, "exchangeRate")]
    public void Constructor_InvalidParameters_Throws(string name, string shortName, int level, int exchangeRate, string testedParamName)
    {
        var ex = Assert.ThrowsAny<ArgumentException>(() => new SingleCurrency(name, shortName, level, exchangeRate));
        Assert.Equal(testedParamName, ex.ParamName);
    }

    [Fact]
    public void SetId_SetsIdValue()
    {
        var currency = new SingleCurrency("Galeon", "gl", 1, 1);

        currency.SetId(42);

        Assert.Equal(42, currency.Id);
    }

    [Fact]
    public void SetId_InvalidValue_Throws()
    {
        var currency = new SingleCurrency("Galeon", "gl", 1, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => currency.SetId(-1));
    }
}
