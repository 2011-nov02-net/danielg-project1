using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;

namespace danielg_projectOne.DataModel.Repositories
{
    public class CustomerRepository: ICustomerRepository
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
        /// Method returns list of Web App customers from the database model
        /// </summary>
        /// <returns></returns>
        public List<CustomerClass> GetAllCustomers()
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get DB Object list of customers
            var dbCustomers = context.Customers.ToList();
            // Make DB list into Web App Customers list
            var appCustomers = dbCustomers.Select(c => new CustomerClass(c.Name, c.Id)).ToList();
            // return customer list
            return appCustomers;
        }


        /// <summary>
        /// Method returns a list of all the cutomers with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CustomerClass> GetAllCustomersByName(string name="")
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get DB Object list of customers
            var dbCustomers = context.Customers.Where(sn => sn.Name.Contains(name));
            // Make DB list into Web App Customers list
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
            // Create a database customer using info from Web App Customer
            Customer dbCustomer = new Customer
            {
                Name = customer.Name
            };

            // Add Customer to database and save change
            context.Customers.Add(dbCustomer);
            context.SaveChanges();
        }

        /// <summary>
        /// Send a Web App order to the database
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
        /// Get amount of GenOrders(Number of orders placed Company wide at all locations)
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
        /// Return a Web App customer based on an int passed in from the customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerClass GetCustomerFromID(int id)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Find the database entry for the specific Customer
            var customer = context.Customers.Find(id);
            // Create the Web App Customer from the database customer
            CustomerClass appCustomer = new CustomerClass(customer.Name, customer.Id);

            return appCustomer;
        }

        /// <summary>
        /// Create a Web App customer complete with their order history
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
                // Get the Web App Customer from the DB
                var tempCust = GetCustomerFromID(order.CustomerId);
                // Get the Web App Location from the DB
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
        /// Get all of the products and amounts that were in a specific order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetOrderDetails(int orderID)
        {
            // Create the context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get the order details that match the orderID
            var listAggOrders = context.AggOrders.Where(o => o.OrderId == orderID);
            // Create a dictionary that is a completely compiled order with product and amount ordered
            Dictionary<string, int> shoppingCart = listAggOrders.ToDictionary(o => o.Product, o => o.Amount);

            return shoppingCart;
        }



        /// <summary>
        /// Returns all orders by a customer, with the details so a price can be calculated
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public IEnumerable<IOrder> GetCustomersOrders(int custID)
        {
            // Create Context 
            using var context = new danielGProj0DBContext(_contextOptions);
            // Compile the database orders that match the Customers ID
            var generalOrders = context.GenOrders.Where(o => o.CustomerId == custID).ToList();
            // Create a list of Web App orders to add orders to
            var aggregatedOrders = new List<IOrder>();
            // Iterate through the Customers orders
            foreach (var order in generalOrders)
            {
                // Create the Customer for the currently iterated order
                var tempCust = GetCustomerFromID(order.CustomerId);
                // Create the Location for the currently iterated order
                var tempLocation = GetStoreFromID(order.StoreId);
                // Create the Web App order to be added to the List of orders
                Order tempOrder = new Order(tempLocation, tempCust, order.Id, order.Date);
                // Find all of the order details that match the OrderID 
                var listAggOrders = context.AggOrders.Where(o => o.OrderId == order.Id);
                // Iterate through the order details 
                foreach (var agOrder in listAggOrders)
                {
                    // Add the aggregateOrders essentially as orders being placed so I can
                    //   Just keep them in memory and move them
                    tempOrder.Customer.ShoppingCart.Add(agOrder.Product, agOrder.Amount);
                }
                // Add each order with details to the list of orders
                aggregatedOrders.Add(tempOrder);
            }
            return aggregatedOrders;
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

        /// <summary>
        /// Create the inventory of a store, based on the storeID
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Dictionary<string, int> CreateStoreInventory(int storeID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == storeID).ToList();
            // create Web App inventory with db inventory pieces

            Dictionary<string, int> inv = itemsInInventory.ToDictionary(i => i.Product, i => i.InStock);

            return inv;
        }

        /// <summary>
        /// Get all of the products from the database and create the Web App products
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
            // Make DB list into Web App Stores List
            var appStores = dbStores.Select(s => new Location(s.Location, s.Id)).ToList();

            return appStores;
        }

        /// <summary>
        /// Create Web App store, from the database. Complete with inventory.
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public Location CreateStoreWithInventory(int storeID)
        {
            // Create Context
            using var context = new danielGProj0DBContext(_contextOptions);
            // Get agg inventory items from the database- this is a stores inventory
            var itemsInInventory = context.AggInventories.Where(i => i.StoreId == storeID).ToList();
            // create Web App inventory with db inventory pieces
            Dictionary<string, int> inv = itemsInInventory.ToDictionary(i => i.Product, i => i.InStock);
            // Create store From Store DBtable
            var dbStore = context.Stores.Where(s => s.Id == storeID);
            // Create the store in the console. Complete with an inventory
            var appStore = dbStore.Select(s => new Location(s.Location, s.Id, inv)).ToList();


            return appStore.First();
        }
    }
}
