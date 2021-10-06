using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Animal
    {
        public int Id { set; get; }
        [Display(Name="Identification number")]
        public int IdentificationNumber { get; set; }
        [Required]
        public string Hb { get; set; }
        [Required]
        public string Rb { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        public TypeOfAnimal TypeOfAnimal { set; get; }
        [Required]
        public bool Gender { get; set; }
        public DateTime ExclusionDate { get; set; }
        [StringLength(1600)]
        [Display(Name="Exclusion Reason")]
        public string ExclusionReason { get; set; }
        [Display(Name="Days in first mating")]
        public int DaysInFirstMating { get; set; }
        [Display(Name="Left tits")]
        public int LeftTits { get; set; }
        [Display(Name="Right Tits")]
        public int RightTits { get; set; }
        [Required]
        [Display(Name="Selection Mark")]
        public string SelectionMark { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        [Display(Name="Registration Number")]
        public int RegistrationNumber { get; set; }
        [Display(Name="Tatoo Number")]
        public int TatooNumber { get; set; }
        [Display(Name="Birth Type")]
        public string BirthType { get; set; }    
        public ICollection<AnimalEvent> Events {set; get; }
            =new List<AnimalEvent>();
    }
}