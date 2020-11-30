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
            List<CustomerClass> custs = Repo.GetAllCustomersByName(search);
            List<CustomerClass> custss = Repo.GetAllCustomers();
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

            try
            {
                
                var storeProducts = Repo.GetProducts();
                var customersOrders = Repo.GetCustomersOrders(id);

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
                // Do error handling
            }
            

            return View(custOrders);
        }


        public IActionResult Create()
        {


            return View();
        }
    }
}
