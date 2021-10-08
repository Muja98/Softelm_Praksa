using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public bool AdministratorFlag { get; set; }
        public bool BuyerFlag  { set; get; }
        public bool FarmerFlag { get; set; }
        public bool WorkerFlag { get; set; }
        public string Description { get; set; }
    }
}
