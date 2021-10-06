using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Possession
    {
        public int Id {set ; get; }
        [Required]
        [StringLength(1000)]
        public string Location {set; get; }

        [Display(Name="X and Y Coordinates")]
        public double Lat {set; get; }

        [Display(Name="X and Y Coordinates")]
        public  double Lng {set; get; }
        [Display(Name="Possession Name")]
        public string Name {set; get; }

        public Farm Farm {set; get; }
        public ICollection<Season> Seasons {set; get; }
            =new List<Season>();
    }
}