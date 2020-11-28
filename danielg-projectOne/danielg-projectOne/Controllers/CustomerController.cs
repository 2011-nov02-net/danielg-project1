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

        public ICustomerRepository Repo { get;  }

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
                // No 
            }


            return View();
        }
    }
}
