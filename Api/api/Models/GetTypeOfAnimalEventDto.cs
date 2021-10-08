using System;

namespace api.Models
{
    public class GetTypeOfAnimalEventDto
    {
        public int Id { get; set; }
        public string Name { set; get;}
        public float Stake { set; get; }
        public DateTime DateOfEvent { set; get; }
        public string ContributionType { set; get; }
        public int Contribution { set; get; }
        public int ProductId { get; set; }
    }
}