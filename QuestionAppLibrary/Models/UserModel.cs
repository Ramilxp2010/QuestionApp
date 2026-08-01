
namespace QuestionAppLibrary.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ObjectIdentifier { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public string Name { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        [BsonElement("AutoredQuestions")]
        public List<BasicQuestionModel> AuthoredQuestions { get; set; } = new();

        public List<BasicAnswerModel> AuthoredAnswers { get; set; } = new();

        public List<BasicAnswerModel> VotedAnswers { get; set; } = new();

        public List<BasicQuestionModel> VotedQuestions { get; set; } = new();
    }
}
