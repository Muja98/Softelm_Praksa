using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Entities
{
    public class JobAdvertisement
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateOfPuplication { get; set; }
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool ActiveFlag { get; set; }
        public Farm Farm { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; }
            = new List<JobApplication>();
    }
}