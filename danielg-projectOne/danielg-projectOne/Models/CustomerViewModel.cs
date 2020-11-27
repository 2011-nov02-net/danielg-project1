using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class CustomerViewModel
    {

        [Display(Name = "ID")]
        [Required]
        public int ID { get; set; }


        [Display(Name = "Name")]
        [Required]
        public string FullName { get; set; }
    }
}
