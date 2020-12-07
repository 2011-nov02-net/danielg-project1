using System.Collections.Generic;
using danielg_projectOne.Library;
using Xunit;

namespace danielg_projectOne.UnitTests
{
    public class CustomerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(10)]
        public void TestOrderTooManyPass(int value)
        {
            var customer = new CustomerClass();

            bool tf = customer.AddToCart("Dumb McFlurry", value);

            Assert.True(tf, "These values are within the range to order");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(11)]
        [InlineData(104)]
        public void TestOrderTooManyFail(int value)
        {
            var customer = new CustomerClass();

            bool tf = customer.AddToCart("Dumb McFlurry", value);

            Assert.False(tf, "These values are too many or too few");
        }


        [Fact]
        public void TestCartGetsCreatedPass()
        {
            Dictionary<string, int> cart = new Dictionary<string, int>() {
                { "Dumb French Fries", 3}
            };
            var customer = new CustomerClass(cart);

            bool cartMade = customer.ShoppingCart["Dumb French Fries"] == 3;

            Assert.True(cartMade, "Cart was succesfully created");
        }

        [Fact]
        public void TestCartGetsCreatedFail()
        {
            Dictionary<string, int> cart = new Dictionary<string, int>() {
                { "Dumb French Fries", 2}
            };
            var customer = new CustomerClass(cart);

            bool cartMade = customer.ShoppingCart["Dumb French Fries"] == 3;

            Assert.False(cartMade, "Cart Should not pass test");
        }

    }
}
