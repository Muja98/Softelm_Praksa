using System;

namespace api.Models
{
    public class GetWorkingTaskDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCration { get; set; }
        public DateTime CompletionDeadline { get; set; }
        public bool Completed { get; set; }

        public int FarmId { get; set; }
    }
}