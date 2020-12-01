using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class PlaceOrderViewModel
    {
        [Display(Name = "ID")]
        public int ID { get; set; }


        [Display(Name = "Name")]
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must only contain letters or spaces")]
        public string FullName { get; set; }

        [Display(Name = "Item")]
        public Dictionary<string, int> StockedItems { get; set; }
    }
}
