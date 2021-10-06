using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using api.Entities;

namespace api.Relations
{
    public class WorksOn
    {
        public int Id { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public float Grade { get; set; }
        public User Worker { get; set; }
        public Farm Farm { get; set; }
    }
}