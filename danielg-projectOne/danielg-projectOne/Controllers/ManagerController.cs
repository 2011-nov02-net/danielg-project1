using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using danielg_projectOne.DataModel;
using danielg_projectOne.DataModel.Repositories;
using danielg_projectOne.Library;
using danielg_projectOne.Models;
using Microsoft.AspNetCore.Mvc;


namespace danielg_projectOne.Controllers
{
    public class ManagerController : Controller
    {
        /// <summary>
        /// Create the Repository field that will search the DB for this controller
        /// </summary>
        public IManagerRepository Repo { get; }

        /// <summary>
        /// Assign a Repository to the Controller 
        /// </summary>
        /// <param name="repo"></param>
        public ManagerController(IManagerRepository repo) =>
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));


        // GET: /<controller>/
        public IActionResult Index(string search = "")
        {
            IEnumerable<StoreViewModel> vmStores = new List<StoreViewModel>();
            try
            {
                // Get the list of all stores matching the sercch string
                List<Location> stores = Repo.GetAllStoresByLocation(search);

                vmStores = stores.Select(s => new StoreViewModel
                {
                    ID = s.Id,
                    Location = s.CityLocation
                });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View(vmStores);
        }

        //GET: Manager/Home/1
        public IActionResult Home(int id = 0)
        {
            if (id == 0)
            {
                // No customer was able to sign in, this shouldnt happen
            }
            // Create list of viewmodel to pass to view method in return
            IEnumerable<StoreOrderViewModel> storeOrders = null;

            try
            {

                var storeProducts = Repo.GetProducts();
                var storesOrders = Repo.GetStoresOrders(id);

                storeOrders = storesOrders.Select(c => new StoreOrderViewModel
                {
                    ID = c.OrderID,
                    Customer = c.Customer.Name,
                    Cost = c.CalculateTotal(storeProducts),
                    Date = c.Date
                });

            }
            catch 
            {
                return RedirectToAction(nameof(Index));
            }


            return View(storeOrders);
        }
    }
}
