using System;
using api.Entities;

namespace api.Models
{
    public class PutAnimalDto
    {
        public int IdentificationNumber { get; set; }
        public string Hb { get; set; }
        public string Rb { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public DateTime ExclusionDate { get; set; }
        public string ExclusionReason { get; set; }
        public int DaysInFirstMating { get; set; }
        public int LeftTits { get; set; }
        public int RightTits { get; set; }
        public string SelectionMark { get; set; }
        public int RegistrationNumber { get; set; }
        public int TatooNumber { get; set; }
        public string BirthType { get; set; }  
    }
}