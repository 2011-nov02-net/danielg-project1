using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class StoreViewModel
    {
        [Display(Name = "ID")]
        [Required]
        public int ID { get; set; }


        [Display(Name = "Location")]
        [Required]
        public string Location { get; set; }
    }
}
