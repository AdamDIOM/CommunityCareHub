using System.ComponentModel.DataAnnotations;

namespace WellnessSite.Models
{
    public class SecQues
    {
        [Key]
        public int SecQuesID { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
