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
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Town { get; set; }
        public string? Postcode { get; set; }
        public string? WebLink { get; set; }
        public string? Other { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageAltText { get; set; }
        public string? Tags { get; set; }
        [Required]
        public bool Accepted { get; set; }
        public string? Maintainer { get; set; }
    }
}
