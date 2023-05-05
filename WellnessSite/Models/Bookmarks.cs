using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellnessSite.Models
{
    public class Bookmarks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string UID { get; set; }
        [Required]
        public int SID { get; set; }
    }
}
