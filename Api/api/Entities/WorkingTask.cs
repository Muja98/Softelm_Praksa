using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace api.Entities
{
    public class WorkingTask
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Date of creation")]
        public DateTime DateOfCreation { get; set; }

        [Required]
        [Display(Name = "Completion deadline")]
        public DateTime CompletionDeadline { get; set; }

        [Required]
        public bool Completed { get; set; }
        
        public User Worker { get; set; }
        public Farm Farm { get; set; }

    }
}