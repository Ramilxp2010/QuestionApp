namespace QuestionAppLibrary.Models
{
    public class BasicQuestionModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Question { get; set; }

        public BasicQuestionModel()
        {

        }

        public BasicQuestionModel(QuestionModel question)
        {
            Id = question.Id;
            Question = question.Question;
        }
    }
}
