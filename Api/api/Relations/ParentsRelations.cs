using api.Entities;

namespace api.Relations
{
    public class ParentsRelations 
    {
        public int Id { set; get; }


        public Animal Child { set; get; }
        public Animal Mather { set; get; }
        public Animal Father { set; get; }
    }
}