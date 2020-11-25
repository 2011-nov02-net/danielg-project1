using System;
namespace danielg_projectOne.Library
{
    public class Product
    {
        // Private fields to store the name and price of the Products
        private string _productName;
        private decimal _price;

        // Constructor creates the product with a name and a price
        public Product(string productName, decimal? price)
        {
            ProductName = productName;
            Price = (decimal)price;
        }

        /// <summary>
        /// Public property to get or set the name of a product
        /// </summary>
        public string ProductName { get => _productName; set => _productName = value; }

        /// <summary>
        /// Public property to get or set the price of a product
        /// </summary>
        public decimal Price { get => _price; set => _price = value; }

    }
}
