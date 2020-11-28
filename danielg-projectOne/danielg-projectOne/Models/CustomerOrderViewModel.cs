using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class CustomerOrderViewModel
    {
        //id, location, total, date
        [Display(Name = "Order ID")]
        [Required]
        public int ID { get; set; }

        [Display(Name = "Location")]
        [Required]
        public string Location { get; set; }

        [Display(Name = "Cost")]
        [Required]
        public decimal Cost { get; set; }

        [Display(Name = "Date")]
        [Required]
        public DateTime Date { get; set; }
    }
}
