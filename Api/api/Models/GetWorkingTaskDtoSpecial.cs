using System;

namespace api.Models
{
    public class GetWorkingTaskDtoSpecial
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime CompletionDeadline { get; set; }
        public bool Completed { get; set; }
        public int WorkerId { get; set; }
        public string Fullname { get; set; }
        public int FarmId{ get; set; }
        public string FarmName { get; set; }
    }
}