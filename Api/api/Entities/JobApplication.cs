using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        
        [Required]
        public string MotivationLetter { get; set; }
        public JobAdvertisement Announcement { get; set; }
        public User User { get; set; }
        public string FarmName { set; get; }
        public string AnnouncementTitle { set; get; }
        public int Accepted { set; get; }
    }
}