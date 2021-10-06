using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using api.Relations;

namespace api.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength=3)]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength=3)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        
        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }
        
        public bool AdministratorFlag { get; set; }
        public bool FarmerFlag { get; set; }
        public bool WorkerFlag { get; set; }
        public bool BuyerFlag { set; get; }

        public string Description { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Farm> Farms { get; set; }
            = new List<Farm>();

        public ICollection<WorksOn> WorksOn { get; set; }
            = new List<WorksOn>();
        
        public ICollection<WorkingTask> WorkingTasks { get; set; }
            = new List<WorkingTask>();
        
        public ICollection<JobApplication> JobApplications { get; set; }
            = new List<JobApplication>();
        
        public ICollection<Judgement> Judgements { get; set; }
            = new List<Judgement>();
    }
};