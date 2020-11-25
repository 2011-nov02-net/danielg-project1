using System;
using System.Collections.Generic;

namespace danielg_projectOne.Library.Order
{

    public class Order : IOrder
    {
        /// <summary>
        /// Private fields to store data pertaining to an order
        /// </summary>
        private Location _location;
        private CustomerClass _customer;
        private DateTime date;
        private decimal cost;
        private int _orderID;

        /// <summary>
        /// Constructor used to create an order. This is the one from the cosole app.
        /// </summary>
        public Order(Location location, CustomerClass customer)
        {
            Location = location;
            Customer = customer;
            Date = DateTime.Now;

        }


        /// <summary>
        /// Constructor used to create an order. This is the one from the database 
        /// </summary>
        public Order(Location location, CustomerClass customer, int orderID, DateTime dateOfOrder)
        {
            Location = location;
            Customer = customer;
            Date = dateOfOrder;
            OrderID = orderID;

        }
        /// <summary>
        /// Constructor to help test methods
        /// </summary>
        public Order()
        {
            Customer = new CustomerClass();
        }

        public Order(CustomerClass customer)
        {
            Customer = customer;
        }

        /// <summary>
        /// roperty to get or set the location of an order
        /// </summary>
        public Location Location { get => _location; set => _location = value; }

        /// <summary>
        /// Property to get or set the customer of an order
        /// </summary>
        public CustomerClass Customer { get => _customer; set => _customer = value; }


        /// <summary>
        /// Property to get/set the date an order was placed
        /// </summary>
        public DateTime Date { get => date; set => date = value; }

        /// <summary>
        /// Property to get or set the cost of the order
        /// </summary>
        public decimal Cost { get => cost; set => cost = value; }
        /// <summary>
        /// OrderID property to get or set the value
        /// </summary>
        public int OrderID { get => _orderID; set => _orderID = value; }


        /// <summary>
        /// Calculate the total of an order, pass in the list of prodcuts from the database
        /// </summary>
        public decimal CalculateTotal(List<Product> products)
        {
            decimal orderTotal = 0.00M;
            // Get a dictionary of products and prices and get a dictionary of currentorder
            // iterate through the order, matching the amount and price by the product
            foreach (var product in Customer.ShoppingCart)
            {
                // get the product name for shopping cart to search in store inventory 
                var productName = product.Key;
                // Return Product that matches productName in shopping cart
                Product productAddSum = products.Find(p => p.ProductName == productName);
                // Get the price of the product according to DB
                var priceOfProduct = productAddSum.Price;
                // Add the total of 
                orderTotal += (priceOfProduct *= product.Value);
            }
            // Set the cost in this class to the sum of all of the products and prices

            return orderTotal;
        }

        /// <summary>
        /// Using method logic to test out methods on class
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int CalaculateTotalOfOneProduct(Product prod)
        {
            var dict = Customer.ShoppingCart;
            int amt = dict[prod.ProductName];
            return (int)(prod.Price *= amt);
        }

        /// <summary>
        /// Method to add products to a customers cart
        /// </summary>
        /// <param name="product"></param>
        /// <param name="amountOrderd"></param>
        public void AddToCustomerCart(Product product, int amountOrderd)
        {
            Customer.ShoppingCart.Add(product.ProductName, amountOrderd);
        }

        /// <summary>
        /// Print details of an order
        /// </summary>
        public void printDetails()
        {
            Console.WriteLine("---------------");
            Console.WriteLine("Current Order");
            foreach (var product in Customer.ShoppingCart)
            {
                if (product.Value > 1)
                {
                    Console.WriteLine($"({product.Value}) {product.Key}");
                }
                if (product.Value == 1)
                {
                    Console.WriteLine($"(1) {product.Key}");
                }

            }
        }
    }
}
