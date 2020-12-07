using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using danielg_projectOne.Library;

namespace danielg_projectOne.Models
{
    public class PlaceOrderViewModel
    {
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "All Products")]
        public IList<ProductViewModel> ProductViewModels { get; set; }

        [Display(Name = "Location")]
        public string StoreLocation { get; set; }
    }
}
