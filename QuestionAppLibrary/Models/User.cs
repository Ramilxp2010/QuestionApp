
namespace QuestionAppLibrary.Models
{
    public class User
    {
        public string Id { get; set; }

        public string ObjectIdentifier { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public string Name { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public List<BasicQuestion> AuthoredQuestions { get; set; } = new();

        public List<BasicAnswer> AuthoredAnswers { get; set; } = new();

        public List<BasicAnswer> VotedAnswers { get; set; } = new();

        public List<BasicQuestion> VotedQuestions { get; set; } = new();
    }
}
