﻿using System;
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
    }
}
