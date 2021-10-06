using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Judgement
    {
        public int Id { get; set; }

        [Required]
        public string Comment { get; set;}

        [Required]
        [Range(1,10)]
        public int Evaluation{ get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}