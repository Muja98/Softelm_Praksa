namespace api.Models
{
    public class GetFarmExtraDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Description { get; set; }
        public int Possessions { set; get; }
        public int TypeOfAnimals {set; get; }
        public int Animals { set; get; }
        public int Workers { set; get; }
        public int Products { set; get; }
    }
}