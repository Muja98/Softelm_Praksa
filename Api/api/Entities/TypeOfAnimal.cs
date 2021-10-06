using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Entities
{
    public class TypeOfAnimal 
    {
        public int Id { set; get; }
        [Display(Name = "Number of animals")]
        public int NumberOfAnimals { set; get; }
        public Farm Farm { set; get; }
        public Breed Breed { get; set; }
        
        public  ICollection<Animal> Animals {set; get; }
            =new List<Animal>();
        public ICollection<TypeOfAnimalEvent> Events {set; get; }
            =new List<TypeOfAnimalEvent>();                             
    }
}