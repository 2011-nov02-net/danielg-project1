using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Amount")]
        public int Amount { get; set; }


        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }
    }
}
