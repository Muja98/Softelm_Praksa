using Microsoft.EntityFrameworkCore;
using api.Entities;
using api.Relations;
using Bogus;
using System.Collections.Generic;

namespace api.Contexts
{
    public class FarmieContext : DbContext
    {
        public FarmieContext(DbContextOptions options) : base (options)
        {}
        
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalEvent> AnimalEvents { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Possession> Possessions { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Judgement> Judgements { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<SeasonEvent> SeasonEvents { get; set; }
        public DbSet<WorkingTask> WorkingTasks { get; set; }
        public DbSet<Transaction> Transactions { set; get;}
        public DbSet<TypeOfAnimal> TypesOfAnimal { get; set; }
        public DbSet<TypeOfAnimalEvent> TypesOfAnimalEvents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ParentsRelations> ParentsRelations { get; set; }
        public DbSet<WorksOn> WorksOn { get; set; }
        public DbSet<JobAdvertisement> JobAdvertisements  { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}