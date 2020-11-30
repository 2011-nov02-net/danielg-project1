using System;
using System.Collections.Generic;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.Library
{ 

        public class CustomerClass
        {
            // Private fields to store data specific to the customer.
            private Dictionary<string, int> shoppingCart;
            private List<IOrder> customersOrders;


            /// <summary>
            /// Constructor giving Customer initial state of full name. Plus
            ///    initialize the pastOrders list.
            ///    Use this constructor when creating a Customer from the ConsoleApp
            /// </summary>
            public CustomerClass(string name)
            {
                Name = name;

            }

            /// <summary>
            /// Constructor to help test methods
            /// </summary>
            /// <param name="dict"></param>
            public CustomerClass(Dictionary<string, int> dict)
            {
                shoppingCart = dict;
            }

            /// <summary>
            /// Constructor to use when creating customers from the database.
            ///     The database is what gives them the ID
            /// </summary>
            public CustomerClass(string name, int id)
            {
                Name = name;
                Id = id;
                ShoppingCart = new Dictionary<string, int>();
                CustomersOrders = new List<IOrder>();
            }

            public CustomerClass()
            {
                ShoppingCart = new Dictionary<string, int>();
            }

            /// <summary>
            /// Property to change or get the value of the shopping cart
            /// </summary>
            public Dictionary<string, int> ShoppingCart { get => shoppingCart; set => shoppingCart = value; }

            /// <summary>
            /// Property to get the name of a customer.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Property to get the name of a customer
            /// </summary>
            public int Id { get; }

            /// <summary>
            /// Property to get or set the customers past orders
            /// </summary>
            public List<IOrder> CustomersOrders { get => customersOrders; set => customersOrders = value; }


            /// <summary>
            /// Print customer details (Name and ID)
            /// </summary>
            public void printDetails()
            {
                Console.WriteLine($"Name: ({Name}) ID: ({Id})");
            }

            /// <summary>
            /// Check whether a customer is trying to order too many of an item
            /// </summary>
            public bool AddToCart(string productName, int amountDesired)
            {
                if (amountDesired > 10)
                {
                    Console.WriteLine($"Too many {productName}'s");
                    return false;
                }
                else if (amountDesired < 0)
                {
                    Console.WriteLine($"Please enter a valid number of {productName}'s");
                    return false;
                }
                ShoppingCart.Add(productName, amountDesired);
                return true;
            }


            public IEnumerable<IOrder> RecallOrders(string id)
            {
                throw new NotImplementedException();
            }
        }
    
}
