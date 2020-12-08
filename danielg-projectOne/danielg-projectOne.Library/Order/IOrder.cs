using System;
using System.Collections.Generic;

namespace danielg_projectOne.Library.Order
{

    public interface IOrder
    {
        /// <summary>
        /// Property to get or set the cost of the order
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Property to get/set the date an order was placed
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Property to get/set the Location of an order
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Property to get/set the Customer of an order8=
        /// </summary>
        public CustomerClass Customer { get; set; }
        /// <summary>
        /// Property to get/set the ID of an order
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// calculate the total cost of an order
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public decimal CalculateTotal(List<Product> products);

        /// <summary>
        /// Print the details of an order
        /// </summary>
        public void printDetails();

        /// <summary>
        /// Method to add products to a customers cart
        /// </summary>
        /// <param name="product"></param>
        /// <param name="amountOrderd"></param>
        public void AddToCustomerCart(Product product, int amountOrderd);

        /// <summary>
        /// Using method logic to test out methods on class
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int CalaculateTotalOfOneProduct(Product prod);

        /// <summary>
        /// Check if an order has any products
        /// </summary>
        /// <returns></returns>
        public bool OrderHasProduct();
    }
}
