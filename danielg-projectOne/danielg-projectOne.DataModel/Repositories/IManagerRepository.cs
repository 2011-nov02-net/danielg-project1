using System.Collections.Generic;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.DataModel.Repositories
{
    public interface IManagerRepository
    {


        /// <summary>
        /// When the manager picks a location to view, call this method to get an entire store.
        ///     with GenOrders and Inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location GetStoreWithOrdersAndInventory(int storeID);


        /// <summary>
        /// Create web app store, from the database. Complete with inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location CreateStoreWithInventory(int storeID);


        /// <summary>
        /// Return a store based on the storeID passed in
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location GetStoreFromID(int storeID);


        /// <summary>
        /// Get a list of web app stores from the database
        /// </summary>
        /// <returns></returns>
        public List<Location> GetStores();


        /// <summary>
        /// Get the number of stores with different location
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfStores();



        /// <summary>
        /// Return a web app customer based on a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerFromID(int id);


        /// <summary>
        /// Get a list of all of the products in the database
        ///     in order to return a list of web app products
        /// </summary>
        /// <returns></returns>
        public List<danielg_projectOne.Library.Product> GetProducts();

        /// <summary>
        /// Find stores based on their location
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Location> GetAllStoresByLocation(string location);

        /// <summary>
        /// Returns all orders from a store, with the details so a price can be calculated
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public IEnumerable<IOrder> GetStoresOrders(int storeID);
    }
}
