using System.ComponentModel.DataAnnotations;

namespace QuestionAppUI.Models
{
    public class CreateQuestionModel
    {
        [Required]
        [MaxLength(100)]
        public string Question { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
