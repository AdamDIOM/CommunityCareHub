using System.ComponentModel.DataAnnotations;

namespace WellnessSite.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string WebLink { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Aim { get; set; }
        [Required]
        public string Referral { get; set; }
        [Required]
        public string Times { get; set; }
    }
}
