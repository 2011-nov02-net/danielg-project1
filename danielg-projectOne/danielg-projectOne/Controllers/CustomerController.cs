using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using danielg_projectOne.DataModel.Repositories;
using danielg_projectOne.Library;
using danielg_projectOne.Models;
using Microsoft.AspNetCore.Mvc;


namespace danielg_projectOne.Controllers
{
    public class CustomerController : Controller
    {
        /// <summary>
        /// Create the Repository field that will search the DB for this controller
        /// </summary>
        public ICustomerRepository Repo { get;  }

        /// <summary>
        /// Assign a Repository to the Controller 
        /// </summary>
        /// <param name="repo"></param>
        public CustomerController(ICustomerRepository repo) =>
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));


        // GET: /Customer?search
        public IActionResult Index(string search = "")
        {
            // Get the list of all customers matching the search string
            List<CustomerClass> custs = Repo.GetAllCustomersByName(search);
            // Populate a list of Customer view model with the list of web app customers
            IEnumerable<CustomerViewModel> vmCusts = custs.Select(c => new CustomerViewModel
            {
                ID = c.Id,
                FullName = c.Name
            });


            return View(vmCusts);
        }

        // GET :/Customer/Home/id
        public IActionResult Home(int id = 0)
        {
            if (id == 0)
            {
                // No customer was able to sign in, this shouldnt happen
            }
            // Create list of viewmodel to pass to view method in return
            IEnumerable<CustomerOrderViewModel> custOrders = null;
            // Add the CustomerID to the viewBag so that I can pass it into the next query string
            ViewBag.CustomerID = id;
            try
            {
                // Get all of the products a store has, for prices
                var storeProducts = Repo.GetProducts();
                // Get the Web App orders from the DB based on the CustomerId that was passed in 
                var customersOrders = Repo.GetCustomersOrders(id);
                // Create a list of 
                custOrders = customersOrders.Select(c => new CustomerOrderViewModel
                {
                    ID = c.OrderID,
                    Location = c.Location.CityLocation,
                    Cost = c.CalculateTotal(storeProducts),
                    Date = c.Date
                });

            }
            catch (Exception ex)
            {
                // log exception
                return RedirectToAction("Index", "Customer");
            }
            

            return View(custOrders);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FullName")] CustomerViewModel custVM)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    // Get the name that the user passed into the view
                    string customerName = custVM.FullName;
                    // Create a Web App customer with the provided name
                    var newCustomer = new CustomerClass(customerName);
                    // Create a customer in the database with the Web App UI
                    Repo.CreateCustomerInDb(newCustomer);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

    }
}
