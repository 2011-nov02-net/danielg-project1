using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using danielg_projectOne.Library.Customer;
using System.Linq;
using danielg_projectOne.Library;
using danielg_projectOne;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.DataModel.Repositories
{
    public class CustomerRepository
    {
        /// <summary>
        /// Create field for context options that will be set using the context options from
        /// Program.cs -> Customer View -> right her
        /// </summary>
        private readonly DbContextOptions<danielGProj0DBContext> _contextOptions;

        /// <summary>
        /// Constructor with context options parameter that gets passed from Customer View
        /// </summary>
        public CustomerRepository(DbContextOptions<danielGProj0DBContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        /// <summary>
        /// Method returns list of console customers from the database model
        /// </summary>
        /// <returns></returns>
        public List<CustomerClass> GetAllCustomers()
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get DB Object list of customers
            var dbCustomers = context.Customers.ToList();
            // Make DB list into Console Customers list
            var appCustomers = dbCustomers.Select(c => new CustomerClass(c.Name, c.Id)).ToList();
            // return customer list
            return appCustomers;
        }

        /// <summary>
        /// Method runs when creating a new 
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomerInDb(CustomerClass customer)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Create a database customer using info from ConsoleApp Customer
            Customer dbCustomer = new Customer
            {
                Name = customer.Name
            };

            // Add Customer to database and save change
            context.Customers.Add(dbCustomer);
            context.SaveChanges();
        }

        /// <summary>
        /// Send a console app to the database
        /// </summary>
        /// <param name="order"></param>
        public void SendGenOrderToDB(IOrder order)
        {
            // Create context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Create a database GenOrder using info from the order passed into this method
            GenOrder dbGeneralOrder = new GenOrder
            {
                CustomerId = order.Customer.Id,
                StoreId = order.Location.Id,
                Cost = order.Cost,
                Date = order.Date

            };

            // Add GenOrder to database and save change
            context.GenOrders.Add(dbGeneralOrder);
            context.SaveChanges();


            // From here, see the last order to get the id and update the other tables
            int lastOrder = GetAmountOfGenOrders();

            // Send the AggOrder of the products to the DB
            SendAggOrder(order, lastOrder);

            // Update the inventory of the store that had a purchase
            UpdateInventory(order);

        }

        /// <summary>
        /// Update the inventory in the database after an order is placed
        /// </summary>
        /// <param name="order"></param>
        public void UpdateInventory(IOrder order)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == order.Location.Id).ToList();
            // Iterate over the items in the order
            foreach (var item in order.Customer.ShoppingCart)
            {
                // Find the item in the list of items that are in stock at that store
                //   then decrement based on the amount ordered
                var currentItem = itemsInInventory.Find(x => x.Product == item.Key);
                currentItem.InStock -= item.Value;

            }
            // Save changes made and send to DB
            context.SaveChanges();
        }

        /// <summary>
        /// Send aggregate Order to database when an order is placed
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderID"></param>
        public void SendAggOrder(IOrder order, int orderID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Create the aggOrders list that need to get sent
            List<AggOrder> aggOrders = new List<AggOrder>();
            // For each value in order.customer.shoppingCart, create a single aggOrder
            //   then add that aggOrder to the database
            foreach (var product in order.Customer.ShoppingCart)
            {
                AggOrder orderDetails = new AggOrder
                {
                    OrderId = orderID,
                    Product = product.Key,
                    Amount = product.Value
                };
                context.AggOrders.Add(orderDetails);
            }
            context.SaveChanges();
        }


        /// <summary>
        /// Get amount of GenOrders(Number of orders placed Company wide)
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfGenOrders()
        {
            // Create context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Count the number of entries in the GenOrder table
            var orderCount = context.GenOrders.Count();

            return orderCount;
        }

        /// <summary>
        /// Get amount of customers that have an account
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfCustomers()
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Count the number of entries in customer table
            var custCount = context.Customers.Count();

            return custCount;

        }

        /// <summary>
        /// Return a Console customer based on an int passed in from the customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerFromID(int id)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Find the database entry for the specific Customer
            var customer = context.Customers.Find(id);
            // Create the Console Customer from the database customer
            CustomerClass appCustomer = new CustomerClass(customer.Name, customer.Id);

            return appCustomer;
        }

        /// <summary>
        /// Create a console customer complete with their order history
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerWithOrders(int custID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get all of the GenOrders from the Database, based on the storeID
            var generalOrders = context.GenOrders.Where(o => o.CustomerId == custID).ToList();
            // Create a list of orders to add to based on the orders to a store
            var aggregatedOrders = new List<IOrder>();
            // for each order to a store, get the details of it.
            foreach (var order in generalOrders)
            {
                // Get the Console Customer from the DB
                var tempCust = GetCustomerFromID(order.CustomerId);
                // Get the Console Location from the DB
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
            // Call GetCustomer From Id passing in the customer id that was searched for
            CustomerClass customerHere = GetCustomerFromID(custID);
            // Add the list of customers orders to the console customer
            customerHere.CustomersOrders = aggregatedOrders;

            return customerHere;
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
            // Convert DBStore into Console Store
            Location appLocation = new Location(dbStore.Location, dbStore.Id);

            return appLocation;
        }


        public Dictionary<string, int> CreateStoreInventory(int storeID)
        {
            // Create empty dictionary to fill in inventory
            Dictionary<string, int> inventory = new Dictionary<string, int>();
            //SortedList<>
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == storeID).ToList();
            // create console inventory with db inventory pieces

            Dictionary<string, int> inv = itemsInInventory.ToDictionary(i => i.Product, i => i.InStock);

            return inv;
        }

        /// <summary>
        /// Get all of the products from the database and create the console app products
        /// </summary>
        /// <returns></returns>
        public List<StoreProject.Library.Product> GetProducts()
        {
            // Create context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get the product names and prices- these two methods just make the "products"
            //  that are the key in the dictionary
            var dbProducts = context.Products.ToList();
            // Make the list of products into an a list of app products
            var appProducts = dbProducts.Select(p => new StoreProject.Library.Product(p.Name, p.Price)).ToList();

            return appProducts;
        }

        /// <summary>
        /// Get the number of locations in database
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
        /// Get a list of all of the stores in the database
        /// </summary>
        /// <returns></returns>
        public List<Location> GetStores()
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Create DB object list of stores
            var dbStores = context.Stores.ToList();
            // Make DB list into Console Stores List
            var appStores = dbStores.Select(s => new Location(s.Location, s.Id)).ToList();

            return appStores;
        }

        /// <summary>
        /// Create console app store, from the database. Complete with inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location CreateStoreWithInventory(int storeID)
        {
            // Create empty dictionary to fill in inventory
            Dictionary<string, int> inventory = new Dictionary<string, int>();
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == storeID).ToList();
            // create console inventory with db inventory pieces
            Dictionary<string, int> inv = itemsInInventory.ToDictionary(i => i.Product, i => i.InStock);
            // Create store From Store DBtable
            var dbStore = context.Stores.Where(s => s.Id == storeID);
            // Create the store in the console. Complete with an inventory
            var appStore = dbStore.Select(s => new Location(s.Location, s.Id, inv)).ToList();


            return appStore.First();
        }
    }
}
