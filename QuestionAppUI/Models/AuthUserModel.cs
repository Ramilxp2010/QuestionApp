using System.ComponentModel.DataAnnotations;

namespace QuestionAppUI.Models
{
    public class AuthUserModel
    {   
        public string Identifier { get; set; }

        [Required]
        public string UserName { get; set;}

        [Required]
        public string Password { get; set;}

        public string Role { get; set; }
    }
}
