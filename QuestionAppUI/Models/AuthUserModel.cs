using System.ComponentModel.DataAnnotations;

namespace QuestionAppUI.Models
{
    public class AuthUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
