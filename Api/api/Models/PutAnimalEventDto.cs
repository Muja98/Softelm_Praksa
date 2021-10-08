using System;

namespace  api.Models
{
    public class PutAnimalEventDto
    {
        public string Name {set; get; }
        public DateTime DateOfEvent {set; get; }
        public float Stake {set; get; }
        public string ContributionType { get; set; }
        public double Contribution { get; set; }
    }
}