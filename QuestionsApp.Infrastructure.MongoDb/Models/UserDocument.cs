
namespace QuestionAppLibrary.Models
{
    public class UserDocument
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
        public List<BasicQuestionDocument> AuthoredQuestions { get; set; } = new();

        public List<BasicAnswerDocument> AuthoredAnswers { get; set; } = new();

        public List<BasicAnswerDocument> VotedAnswers { get; set; } = new();

        public List<BasicQuestionDocument> VotedQuestions { get; set; } = new();
    }
}
