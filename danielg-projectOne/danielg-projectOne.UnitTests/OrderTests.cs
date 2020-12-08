using System.Collections.Generic;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;
using Xunit;


namespace danielg_projectOne.UnitTests
{
    public class OrderTests
    {

        [Fact]
        public void TestCartWillFillFail()
        {
            var order = new Order();

            Product prod1 = new Product("Dumb Big Mac", 2);

            order.AddToCustomerCart(prod1, 1);

            bool filled = order.Customer.ShoppingCart["Dumb Big Mac"] == 2;

            Assert.False(filled, "Cart was not succesfully filled");
        }

        [Fact]
        public void TestCartWillFillPass()
        {
            var order = new Order();

            Product prod1 = new Product("Dumb Big Mac", 2);

            order.AddToCustomerCart(prod1, 1);

            bool filled = order.Customer.ShoppingCart["Dumb Big Mac"] == 1;

            Assert.True(filled, "Cart was succesfully filled");
        }


        [Fact]
        public void TestGetOrderTotal()
        {
            var cart = new Dictionary<string, int>()
            {
                { "Dumb Big Mac", 1}
            };
            var customer = new CustomerClass(cart);

            var order = new Order(customer);

            Product prod1 = new Product("Dumb Big Mac", 3);

            var amt = order.CalaculateTotalOfOneProduct(prod1);

            bool equal = amt == 3;

            Assert.True(equal, "Price should be 3");
        }

        [Fact]
        public void TestGetOrderTotalFail()
        {
            var cart = new Dictionary<string, int>()
            {
                { "Dumb Big Mac", 1}
            };
            var customer = new CustomerClass(cart);

            var order = new Order(customer);

            Product prod1 = new Product("Dumb Big Mac", 2);

            var amt = order.CalaculateTotalOfOneProduct(prod1);

            bool equal = amt == 3;

            Assert.False(equal, "Price should not be 3");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-153)]
        public void TestTotalAmountOfProductsTrue(int value)
        {
            var cart = new Dictionary<string, int>()
            {
                { "Dumb Big Mac", value}
            };
            var customer = new CustomerClass(cart);

            var order = new Order(customer);

            bool equal = order.OrderHasProduct();

            Assert.False(equal, "Order Has No Products");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(9)]
        [InlineData(12)]
        public void TestTotalAmountOfProductsFalse(int value)
        {
            var cart = new Dictionary<string, int>()
            {
                { "Dumb Big Mac", value}
            };
            var customer = new CustomerClass(cart);

            var order = new Order(customer);

            bool equal = order.OrderHasProduct();

            Assert.True(equal, "Order Has at Least One Product");
        }


    }
}
