using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using danielg_projectOne.Library;

namespace danielg_projectOne.Models
{
    public class PlaceOrderViewModel
    {
        [Display(Name = "Name")]
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must only contain letters or spaces")]
        public string FullName { get; set; }

        [Display(Name = "Item")]
        public Dictionary<string, int> StockedItems { get; set; }

        [Display(Name = "All Products")]
        public IList<ProductViewModel> ProductViewModels { get; set; }

        [Display(Name = "Location")]
        public Location StoreLocation { get; set; }
    }
}
