using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellnessSite.Models
{
    public class Bookmarks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BookmarkID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public int ServiceID { get; set; }
    }
}
