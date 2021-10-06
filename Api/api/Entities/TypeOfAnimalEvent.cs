using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Entities
{
    public class TypeOfAnimalEvent
    {
        public int Id { set; get; }

        [Required]
        [StringLength(30, MinimumLength=3)]
        public string Name { set; get;}

        public float Stake { set; get; }

        [Required]
        [Display(Name ="Date of event")]
        public DateTime DateOfEvent { set; get; }

        [Required]
        public string ContributionType { get; set; }

        [Required]
        public double Contribution { get; set; }
        public TypeOfAnimal TypeOfAnimal {set; get; }
        public Product Product { get; set; }
    }
}