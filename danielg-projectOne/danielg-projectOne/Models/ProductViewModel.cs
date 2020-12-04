using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Amount")]
        [Required]
        [Range(0, 10, ErrorMessage = "Please Select a Number to Order: 0-10")]
        public int Amount { get; set; }


        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }
    }
}
