using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Season 
    {
        public int Id {set; get; }

        [Required]
        [StringLength(100, MinimumLength=3)]
        public string Name {set; get; }

        [Required]
        public bool State {set; get; }

        [Required]
        [Display(Name="Season started")]
        public DateTime SeasonStarted {set; get; }
        public string Agriculture {set; get; }
        public Possession Possession {set; get; }
        public ICollection<SeasonEvent> SeasonEvents {set; get; } 
                =new List<SeasonEvent>();
    }
}