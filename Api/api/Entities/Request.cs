using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Request
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public bool Reviewed { get; set; } = false;
    }
}