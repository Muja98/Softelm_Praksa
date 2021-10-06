using System;

namespace api.Models
{
    public class PutTypeOfAnimalEventDto
    {
        public string Name { set; get;}
        public float Stake { set; get; }
        public DateTime DateOfEvent { set; get; }
        public string ContributionType { set; get; }
        public int Contribution { set; get; }
    }
}