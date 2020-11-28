using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class OrderDetailsViewModel
    {
        [Display(Name = "Product Name")]
        [Required]
        public string Product { get; set; }

        [Display(Name = "Amount")]
        [Required]
        public int Amount { get; set; }
    }
}
