using System;
using System.Collections.Generic;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.Library
{
    public class Location
    {

        /// <summary>
        /// Private fields to store city, the inventory dictionary, and the list of orders.
        /// </summary>
        private string _city;
        private Dictionary<string, int> _inventory;
        private List<IOrder> orders;


        /// <summary>
        /// Constructor to make new Locations
        /// </summary>
        public Location(string city, Dictionary<string, int> inventory)
        {
            City = city;
            Inventory = inventory;
            Orders = new List<IOrder>();
        }

        /// <summary>
        /// Constructo used to create a Location with a set location and id
        /// </summary>
        /// <param name="location"></param>
        /// <param name="id"></param>
        public Location(string location, int id)
        {
            CityLocation = location;
            Id = id;

        }

        /// <summary>
        /// Constructor to create store from db with an inventory.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="id"></param>
        /// <param name="inventory"></param>
        public Location(string location, int id, Dictionary<string, int> inventory)
        {
            CityLocation = location;
            Id = id;
            Inventory = inventory;
        }

        /// <summary>
        /// Empty constructor to help with testing methods
        /// </summary>
        public Location()
        {

        }


        /// <summary>
        /// Property to get and set the City.
        /// </summary>
        public string City { get => _city; set => _city = value; }

        /// <summary>
        /// Property to get and set the inventory
        /// </summary>
        public Dictionary<string, int> Inventory { get => _inventory; set => _inventory = value; }

        /// <summary>
        /// Property to get and set the Orders list
        /// </summary>
        public List<IOrder> Orders { get => orders; set => orders = value; }

        /// <summary>
        /// Generated property to set the city of a Store
        /// </summary>
        public string CityLocation { get; }
        /// <summary>
        /// Generated Property to set the ID of a store
        /// </summary>
        public int Id { get; }


        /// <summary>
        /// Check if store has enough inventory to staisfy an order
        /// </summary>
        public bool CheckInventory(IOrder order)
        {
            foreach (var product in order.Customer.ShoppingCart)
            {
                // set quantity stocked to the value stored in inventory at product.key
                var quantityStocked = Inventory[product.Key];
                // If there are more in the order than there are in stock
                if (product.Value > quantityStocked)
                {
                    return false;
                }

            }
            // If there is enough in stock for all of the 
            return true;
        }

        /// <summary>
        /// Location calls this method to update 
        /// </summary>
        /// <param name="order"></param>
        /// 
        public bool OrderPlaced(IOrder order)
        {
            // Run this method to check if there is enough in stock
            if (CheckInventory(order))
            {
                // For each item in the order
                foreach (var product in order.Customer.ShoppingCart)
                {
                    // Subtract from inventory the amount of product that was ordered
                    Inventory[product.Key] -= product.Value;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method that can be run to check if the store location will be under 0.
        ///     this is not allowed so returning false cancels tranaction
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool CheckInventoryValid(int inventory)
        {
            if (inventory < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Take the details of a Location and print them out
        /// </summary>
        public void PrintDetails()
        {
            Console.WriteLine($"Dumb Mcdonalds in: ({CityLocation}), StoreID: ({Id})");
        }


    }
}
