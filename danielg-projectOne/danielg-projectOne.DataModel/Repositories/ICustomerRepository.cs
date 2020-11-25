using System;
using System.Collections.Generic;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.DataModel.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Method returns list of web app customers from the database model
        /// </summary>
        /// <returns></returns>
        public List<CustomerClass> GetAllCustomers();


        /// <summary>
        /// Method runs when creating a new customer in the database
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomerInDb(CustomerClass customer);


        /// <summary>
        /// Send a web app order to the database
        /// </summary>
        /// <param name="order"></param>
        public void SendGenOrderToDB(IOrder order);


        /// <summary>
        /// Update the inventory in the database after an order is placed
        /// </summary>
        /// <param name="order"></param>
        public void UpdateInventory(IOrder order);


        /// <summary>
        /// Send aggregate Order to database when an order is placed
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderID"></param>
        public void SendAggOrder(IOrder order, int orderID);


        /// <summary>
        /// Get amount of GenOrders(Number of orders placed Company wide)
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfGenOrders();


        /// <summary>
        /// Get amount of customers that have an account
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfCustomers();


        /// <summary>
        /// Return a Web App customer based on an int passed in from the customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerFromID(int id);


        /// <summary>
        /// Create a Web App customer complete with their order history
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerWithOrders(int custID);


        /// <summary>
        /// Return a store based on the storeID passed in
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location GetStoreFromID(int storeID);


        /// <summary>
        /// Create the inventory of a store, based on the storeID
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Dictionary<string, int> CreateStoreInventory(int storeID);


        /// <summary>
        /// Get all of the products from the database and create the Web App products
        /// </summary>
        /// <returns></returns>
        public List<danielg_projectOne.Library.Product> GetProducts();


        /// <summary>
        /// Get the number of locations in database
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfStores();


        /// <summary>
        /// Get a list of all of the stores in the database
        /// </summary>
        /// <returns></returns>
        public List<Location> GetStores();


        /// <summary>
        /// Create Web App store, from the database. Complete with inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location CreateStoreWithInventory(int storeID);










    }
}
