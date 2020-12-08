using danielg_projectOne.Library;
using Xunit;


namespace danielg_projectOne.UnitTests
{
    public class LocationTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-193)]
        public void TestKeepInventoryNotNegativeFail(int value)
        {
            var location = new Location();

            bool tf = location.CheckInventoryValid(value);

            Assert.False(tf, "Inventory should always be positive or 0");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(19)]
        [InlineData(10754)]
        public void TestKeepInventoryNotNegativePass(int value)
        {
            var location = new Location();

            bool tf = location.CheckInventoryValid(value);

            Assert.True(tf, "Inventory should always be positive or 0");
        }
    }
}