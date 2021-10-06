using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100,MinimumLength=3)]
        public string Name { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int InStock { get; set; }

        public string Description { get; set; }

        public ProductType Type { get; set; }
        public Farm Farm { get; set; }
        public byte [] Image { set; get; }

        public ICollection<Transaction> Transactions { get; set; }
            =  new List<Transaction>();

        public ICollection<Judgement> Judgements { get; set; }
            = new List<Judgement>();
        
        public ICollection<SeasonEvent> SeasonEvents { get; set; }
            = new List<SeasonEvent>();

        public ICollection<AnimalEvent> AnimalEvents { get; set;}
            = new  List<AnimalEvent>();

        public ICollection<TypeOfAnimalEvent> TypeOfAnimalEvents { get; set; }
            = new List<TypeOfAnimalEvent>();
    }
}