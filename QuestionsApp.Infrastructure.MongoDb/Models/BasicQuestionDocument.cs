namespace QuestionAppLibrary.Models
{
    public class BasicQuestionDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string QuestionText { get; set; }

        public BasicQuestionDocument()
        {

        }

        public BasicQuestionDocument(QuestionDocument question)
        {
            Id = question.Id;
            QuestionText = question.QuestionText;
        }
    }
}
