using System;

namespace api.Models
{
    public class GetAnimalEventDto
    {
        public int Id {set; get; }
        public string Name {set; get; }
        public DateTime DateOfEvent {set; get; }
        public float Stake {set; get; }
        public string ContributionType { get; set; }
        public double Contribution { get; set; }
        public int ProductId { get; set; }
    }
}