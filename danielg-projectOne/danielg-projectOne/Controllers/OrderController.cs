using System;
using System.Collections.Generic;
using System.Linq;
using danielg_projectOne.DataModel.Repositories;
using danielg_projectOne.Library;
using danielg_projectOne.Library.Order;
using danielg_projectOne.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace danielg_projectOne.Controllers
{
    public class OrderController : Controller
    {
        public ICustomerRepository Repo { get; }

        public OrderController(ICustomerRepository repo) =>
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET: Order/Details/1
        public IActionResult Details(int id = 0)
        {
            // Create the list of pieces of the order to add to
            List<OrderDetailsViewModel> orderViewModels = new List<OrderDetailsViewModel>();

            try
            {
                if (id == 0)
                {
                    // if orderID was not sent, this should not happen
                }
                // Create the cart that was already ordered
                Dictionary<string, int> cart = Repo.GetOrderDetails(id);
                
                // Iterate through the cart 
                foreach (var item in cart)
                {
                    // Create each piece of the order for the viewmodel to show to the user
                    OrderDetailsViewModel orderPiece = new OrderDetailsViewModel
                    {
                        Product = item.Key,
                        Amount = item.Value
                    };
                    // Add each piece of the order to the viewmodel to show to the user
                    orderViewModels.Add(orderPiece);
                }
            }
            catch 
            {
                return RedirectToAction("Home", "Customer", new { id = id});
            }
            return View(orderViewModels);
        }

        // GET: Order/MakeOrder/1
        public IActionResult MakeOrder(int id = 0)
        {
            IEnumerable<StoreViewModel> vmStores = new List<StoreViewModel>();
            try
            {
                int customerID = id;
                ViewBag.CustomerID = id;
                List<Location> stores = Repo.GetStores();
                vmStores = stores.Select(s => new StoreViewModel
                {
                    ID = s.Id,
                    Location = s.CityLocation
                });
            }
            catch
            {
                return RedirectToAction("Index", "Customer");
            }
            return View(vmStores);
        }


        // GET: Order/OrderProducts/1?storeID=1
        public IActionResult OrderProducts(int id = 0, int storeID = 0)
        {
            PlaceOrderViewModel poVM = new PlaceOrderViewModel();

            try
            {
                // First, I am going to get the customer...
                var currentCustomer = Repo.GetCustomerFromID(id);
                TempData["currentCustomer"] = id;
                // ...And the store
                var currentLocation = Repo.CreateStoreWithInventory(storeID);
                TempData["currentStore"] = storeID;
                List<ProductViewModel> products = new List<ProductViewModel>();

                foreach (var item in currentLocation.Inventory)
                {
                    var prod = new ProductViewModel
                    {
                        ProductName = item.Key,
                        Amount = 0
                    };
                    products.Add(prod);
                }

                // Create the ViewModel to send to the View. It should have the inventory at least
                poVM = new PlaceOrderViewModel
                {
                    ProductViewModels = products,
                    FullName = currentCustomer.Name,
                    StoreLocation = currentLocation.CityLocation
                };
            }
            catch
            {
                return RedirectToAction("Home", "Customer", new { id = id });
            }
            return View(poVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Order/OrderProducts/1?storeID=1
        public IActionResult OrderProducts([Bind("ProductViewModels")] PlaceOrderViewModel poVM)
        {
            int currentCustomer = (int)TempData["currentCustomer"];
            try
            {
                if (ModelState.IsValid)
                {

                    // Products and Amounts that just got ordered
                    var productVM = poVM.ProductViewModels;

                    // Get customer ID from TempData then use Repo to get the Web App customer
                    var currentCustomerSignIn = Repo.GetCustomerFromID((int)currentCustomer);

                    // Add the products that just got ordered to the customers shopping cart 
                    foreach (var prod in productVM)
                    {
                        currentCustomerSignIn.AddToCart(prod.ProductName, prod.Amount);
                    }
                    // Get StoreID from TempData then use Repo to get the WebApp Store
                    var currentStore = TempData["currentStore"];
                    var currentStoreChosen = Repo.CreateStoreWithInventory((int)currentStore);

                    // Make a new order instance with at the currentStore by the currentCustomer
                    IOrder thisOrder = new Order(currentStoreChosen, currentCustomerSignIn);

                    // Get all of the products available(for prices) 
                    List<Product> products = Repo.GetProducts();
                    // Use the prices of the products to calculate the cost of the order
                    thisOrder.CalculateTotal(products);

                    // Make a bool that checks if the store has enough inventory to fulfill the order
                    bool inventorySufficient = currentStoreChosen.OrderPlaced(thisOrder);
                    bool orderHasProducts = thisOrder.OrderHasProduct();

                    if (orderHasProducts)
                    {
                        // If inventory is large enough, place the order
                        if (inventorySufficient)
                        {
                            Repo.SendGenOrderToDB(thisOrder);
                        }
                    }
                }
            }
            catch
            {
                return RedirectToAction("Index", "Customer");
            }
            return RedirectToAction("Home", "Customer", new { id = currentCustomer });
        }
    }
}
