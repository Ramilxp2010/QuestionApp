namespace QuestionAppLibrary.Models
{
    public class BasicAnswerDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AnswerText { get; set; }

        public BasicAnswerDocument()
        {
        }

        public BasicAnswerDocument(AnswerDocument answerModel)
        {
            Id = answerModel.Id;
            AnswerText = answerModel.AnswerText;
        }
    }
}
