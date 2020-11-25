using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using danielg_projectOne.DataModel.Repositories;
using danielg_projectOne.Library;
using Microsoft.AspNetCore.Mvc;


namespace danielg_projectOne.Controllers
{
    public class CustomerController : Controller
    {

        public ICustomerRepository Repo { get;  }

        public CustomerController(ICustomerRepository repo) =>
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));


        // GET: /<controller>/
        public IActionResult Index()
        {
            List<CustomerClass> custs = Repo.GetAllCustomers();
            Console.WriteLine("Button Clicked");
            return View();
        }
    }
}
