using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using api.Relations;

namespace api.Entities
{
    public class Farm
    {
        public int Id { get; set; } 
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }
        public string Description { get; set; }

        public User Farmer  { get; set; }

        public ICollection<Possession> Possessions {set; get; }
            =new List<Possession>();
        public ICollection<TypeOfAnimal> TypeOfAnimals {set; get; }
            =new List<TypeOfAnimal>();
        public ICollection<Product> Products { get; set; }
            = new List<Product>();
        public ICollection<WorksOn> WorksOn { get; set; }
            = new List<WorksOn>();
        public ICollection<WorkingTask> WorkingTasks {set; get; } 
            =new List<WorkingTask>();
        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();
        public ICollection<JobAdvertisement> Announcements { get; set; }
            = new List<JobAdvertisement>();
            
    }
}