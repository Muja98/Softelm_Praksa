using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Breed
    {
        public int Id { get; set; }

        [Required]
        public string Species { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<TypeOfAnimal>  TypesOfAnimal { get; set; }
            = new List<TypeOfAnimal>();
    }
}