
namespace QuestionAppLibrary.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ObjectIdentifier { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public List<BasicQuestionModel> AutoredQuestions { get; set; } = new();

        public List<BasicAnswerModel> AuthoredAnswers { get; set; } = new();

        public List<BasicAnswerModel> VotedAnswers { get; set; } = new();

        public List<BasicQuestionModel> VotedQuestions { get; set; } = new();
    }
}
