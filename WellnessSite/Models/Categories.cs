using System.ComponentModel.DataAnnotations;

namespace WellnessSite.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
