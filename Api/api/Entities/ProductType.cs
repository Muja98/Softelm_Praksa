using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Entities
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public ICollection<Product> Products{ get; set; }
    }
}