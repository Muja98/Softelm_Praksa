using System.ComponentModel.DataAnnotations;
using System;

namespace api.Entities
{
    public class SeasonEvent
    {
        public int Id {set; get; }

        [Required]
        public DateTime Date {  get; set; }
        
        [Required]
        public string Description {set; get; }
        
        public float Stake {set; get; }
        public float Contribution {set; get; }

        public Product ForProduct { get; set; }
        public Season Season {set; get; }
    }
}