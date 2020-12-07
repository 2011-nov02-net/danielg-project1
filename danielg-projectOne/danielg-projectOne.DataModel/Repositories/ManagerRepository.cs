using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;


namespace danielg_projectOne.DataModel.Repositories
{

    public class ManagerRepository: IManagerRepository
    {
        /// <summary>
        /// Create field for context options that will be set using the context options from
        /// Program.cs -> Customer View -> right her
        /// </summary>
        private readonly DbContextOptions<danielGProj0DBContext> _contextOptions;


        /// <summary>
        /// Constructor with context options parameter that gets passed from Customer View
        /// </summary>
        public ManagerRepository(DbContextOptions<danielGProj0DBContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }


        /// <summary>
        /// When the manager picks a location to view, call this method to get an entire store.
        ///     with GenOrders and Inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location GetStoreWithOrdersAndInventory(int storeID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get all of the GenOrders from the Database, based on the storeID
            var generalOrders = context.GenOrders.Where(o => o.StoreId == storeID).ToList();
            // Create a list of orders to add to based on the orders to a store
            var aggregatedOrders = new List<IOrder>();
            // for each order to a store, get the details of it.
            foreach (var order in generalOrders)
            {
                // Get the web app Customer from the DB
                var tempCust = GetCustomerFromID(order.CustomerId);
                // Get the web app Location from the DB
                var tempLocation = GetStoreFromID(order.StoreId);
                // Add Location and Customer to the order
                Order tempOrder = new Order(tempLocation, tempCust, order.Id, order.Date);
                // Create list of all the aggregateOrders from a store Location 
                var listAggOrders = context.AggOrders.Where(o => o.OrderId == order.Id);
                foreach (var agOrder in listAggOrders)
                {
                    // Add the aggregateOrders essentially as orders being placed so I can
                    //   Just keep them in memory and move them
                    tempOrder.Customer.ShoppingCart.Add(agOrder.Product, agOrder.Amount);
                }
                // Add each order with details to 
                aggregatedOrders.Add(tempOrder);
            }
            // Call CreateStoreWithInventory so I can just return an entire store rather than just the orders to that store.
            Location chosenLocation = CreateStoreWithInventory(storeID);
            // Add the list of previous orders to that store
            chosenLocation.Orders = aggregatedOrders;
            return chosenLocation;
        }



        /// <summary>
        /// Create web app store, from the database. Complete with inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location CreateStoreWithInventory(int storeID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == storeID).ToList();
            // create web app inventory with db inventory pieces
            Dictionary<string, int> inv = itemsInInventory.ToDictionary(i => i.Product, i => i.InStock);
            // Create store From Store DBtable
            var dbStore = context.Stores.Where(s => s.Id == storeID);
            // Create the store in the web app. Complete with an inventory
            var appStore = dbStore.Select(s => new Location(s.Location, s.Id, inv)).ToList();


            return appStore.First();
        }


        /// <summary>
        /// Return a store based on the storeID passed in
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location GetStoreFromID(int storeID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get "Stores" based on ID
            var dbStore = context.Stores.Where(s => s.Id == storeID).First();
            // Convert DBStore into web app Store
            Location appLocation = new Location(dbStore.Location, dbStore.Id);

            return appLocation;
        }

        /// <summary>
        /// Get a list of web app stores from the database
        /// </summary>
        /// <returns></returns>
        public List<Location> GetStores()
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Create DB object list of stores
            var dbStores = context.Stores.ToList();
            // Make DB list into web app Stores List
            var appStores = dbStores.Select(s => new Location(s.Location, s.Id)).ToList();

            return appStores;
        }

        /// <summary>
        /// Get the number of stores with different location
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfStores()
        {
            //Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Count the number of entries in the stores table
            var storeCount = context.Stores.Count();

            return storeCount;
        }

        /// <summary>
        /// Return a web app customer based on a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerFromID(int id)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Find the database entry for the specific Customer
            var customer = context.Customers.Find(id);
            // Create the web app Customer from the database customer
            CustomerClass appCustomer = new CustomerClass(customer.Name, customer.Id);

            return appCustomer;
        }

        /// <summary>
        /// Get a list of all of the products in the database
        ///     in order to return a list of web app products
        /// </summary>
        /// <returns></returns>
        public List<danielg_projectOne.Library.Product> GetProducts()
        {
            // Create context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get the product names and prices- these two methods just make the "products"
            //  that are the key in the dictionary
            var dbProducts = context.Products.ToList();
            // Make the list of products into an a list of app products
            var appProducts = dbProducts.Select(p => new danielg_projectOne.Library.Product(p.Name, p.Price)).ToList();

            return appProducts;
        }

        /// <summary>
        /// Find stores based on their location
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Location> GetAllStoresByLocation(string location="")
        {
            // Create context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get Db object list of stores
            var dbStores = context.Stores.Where(s => s.Location.Contains(location));
            // Make DB list into web app stores list
            var appStores = dbStores.Select(s => new Location(s.Location, s.Id)).ToList();
            // return list of web app customers
            return appStores;
        }


        /// <summary>
        /// Returns all orders from a store, with the details so a price can be calculated
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public IEnumerable<IOrder> GetStoresOrders(int storeID)
        {
            using var context = new danielGProj0DBContext(_contextOptions);

            var generalOrders = context.GenOrders.Where(o => o.StoreId == storeID).ToList();

            var aggregatedOrders = new List<IOrder>();

            foreach (var order in generalOrders)
            {
                var tempCust = GetCustomerFromID(order.CustomerId);

                var tempLocation = GetStoreFromID(order.StoreId);

                Order tempOrder = new Order(tempLocation, tempCust, order.Id, order.Date);

                var listAggOrders = context.AggOrders.Where(o => o.OrderId == order.Id);

                foreach (var agOrder in listAggOrders)
                {
                    // Add the aggregateOrders essentially as orders being placed so I can
                    //   Just keep them in memory and move them
                    tempOrder.Customer.ShoppingCart.Add(agOrder.Product, agOrder.Amount);
                }
                // Add each order with details to 
                aggregatedOrders.Add(tempOrder);
            }
            return aggregatedOrders;
        }
    }
}
