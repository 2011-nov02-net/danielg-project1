using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using danielg_projectOne.DataModel.Repositories;
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
        public IActionResult Index()
        {


            return View();
        }
    }
}
