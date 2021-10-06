using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace api.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Note { get; set; }

        [Required]
        [Display(Name = "Time and date")]
        public DateTime TimeAndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "Unit price")]
        [Range(1, float.MaxValue)]
        public float UnitPrice { get; set; }
        public string ProductName { set; get; }
        public Product Product { get; set; }
        public Farm Farm { get; set; }
        
    }
}