using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class AnimalEvent 
    {
        public int Id {set; get; }
        [Required]
        [StringLength(1000)]
        public string Name {set; get; }
        [Required]
        [Display(Name="Date of event")]
        public DateTime DateOfEvent {set; get; }

        public float Stake {set; get; }

        [Required]
        public string ContributionType { get; set; }

        [Required]
        public double Contribution { get; set; }
        
        public Animal Animal {set; get; }

        public Product Product { get; set; }
    }
}