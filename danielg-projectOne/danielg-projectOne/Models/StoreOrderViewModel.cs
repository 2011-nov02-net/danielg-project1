using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class StoreOrderViewModel
    {
        //id, Customer, total, date
        [Display(Name = "Store ID")]
        [Required]
        public int ID { get; set; }

        [Display(Name = "Customer")]
        [Required]
        public string Customer { get; set; }

        [Display(Name = "Cost")]
        [Required]
        public decimal Cost { get; set; }

        [Display(Name = "Date")]
        [Required]
        public DateTime Date { get; set; }
    }
}
