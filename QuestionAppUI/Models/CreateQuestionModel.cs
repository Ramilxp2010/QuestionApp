using System.ComponentModel.DataAnnotations;

namespace QuestionAppUI.Models
{
    public class QuestionVM
    {
        [Required]
        [MaxLength(100)]
        public string Question { get; set; }

        [Required]
        [Display(Name = "Categories")]
        public HashSet<string> CategoryIds { get; set; } = new HashSet<string>();

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
