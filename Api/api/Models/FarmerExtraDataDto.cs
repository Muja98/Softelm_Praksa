using System.Collections.Generic;

namespace  api.Models
{
    public class FarmerExtraDataDto
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }

        public List<WorkerExtraDataDto> Workers { get; set; }
            = new List<WorkerExtraDataDto>();
    }
}