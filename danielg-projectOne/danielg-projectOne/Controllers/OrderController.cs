using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using danielg_projectOne.DataModel.Repositories;
using danielg_projectOne.Library;
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


        public IActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                // if orderID was not sent, this should not happen
            }

            Dictionary<string, int> cart = Repo.GetOrderDetails(id);

            List<OrderDetailsViewModel> orderViewModels = new List<OrderDetailsViewModel>();

            foreach (var item in cart)
            {

                OrderDetailsViewModel orderPiece = new OrderDetailsViewModel
                {
                    Product = item.Key,
                    Amount = item.Value
                };
                orderViewModels.Add(orderPiece);
                //orderViewModels.Append(orderPiece);
                //orderViewModels.Append(new OrderDetailsViewModel
                //{
                //    Product = item.Key,
                //    Amount = item.Value
                //});
            }

            return View(orderViewModels);
        }
    }
}
