namespace QuestionAppLibrary.Models
{
    public class BasicAnswerModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Answer { get; set; }

        public BasicAnswerModel()
        {
        }

        public BasicAnswerModel(AnswerModel answerModel)
        {
            Id = answerModel.Id;
            Answer = answerModel.Answer;
        }
    }
}
