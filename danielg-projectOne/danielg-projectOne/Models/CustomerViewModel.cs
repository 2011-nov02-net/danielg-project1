using System;
using System.ComponentModel.DataAnnotations;

namespace danielg_projectOne.Models
{
    public class CustomerViewModel
    {

        [Display(Name = "ID")]
        public int ID { get; set; }


        [Display(Name = "Name")]
        [Required]
        [RegularExpression(@"[A-Za-z]", ErrorMessage = "Name must only contain letters")]
        public string FullName { get; set; }
    }
}
