using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Get the details of an order(Products and amounts) based on the orderID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                // if orderID was not sent, this should not happen
            }
            // Create the cart that was already ordered
            Dictionary<string, int> cart = Repo.GetOrderDetails(id);
            // Create the list of pieces of the order to add to
            List<OrderDetailsViewModel> orderViewModels = new List<OrderDetailsViewModel>();
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
            return View(orderViewModels);
        }

        
        public IActionResult MakeOrder(int id = 0)
        {
            int customerID = id;
            ViewBag.CustomerID = id;
            List<Location> stores = Repo.GetStores();
            IEnumerable<StoreViewModel> vmStores = stores.Select(s => new StoreViewModel
            {
                ID = s.Id,
                Location = s.CityLocation
            });
            return View(vmStores);
        }


        public IActionResult OrderProducts(int id = 0, int storeID = 0)
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
                var prod = new ProductViewModel {
                    ProductName = item.Key,
                    Amount = 0
                };
                products.Add(prod);
            }

            // Create the ViewModel to send to the View. It should have the inventory at least
            PlaceOrderViewModel poVM = new PlaceOrderViewModel
            {
                ProductViewModels = products,
                FullName = currentCustomer.Name,
                StoreLocation = currentLocation.CityLocation
            };
            
            return View(poVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderProducts([Bind("ProductViewModels")] PlaceOrderViewModel poVM)
        {
            int currentCustomer = 0;
            try
            {
                if (ModelState.IsValid)
                {

                    // Products and Amounts that just got ordered
                    var productVM = poVM.ProductViewModels;

                    // Get customer ID from TempData then use Repo to get the Web App customer
                    currentCustomer = (int)TempData["currentCustomer"];
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
                    // If inventory is large enough, place the order
                    if (inventorySufficient)
                    {
                        Repo.SendGenOrderToDB(thisOrder);
                    }

                }
            }
            catch (Exception ex)
            {
                // log error
                return RedirectToAction("Index", "Customer");
            }
            return RedirectToAction("Home", "Customer", new { id = currentCustomer });
        }
    }
}
