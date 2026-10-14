namespace QuestionAppLibrary.Models
{
    public class BasicAnswer
    {
        public string Id { get; set; }

        public string AnswerBody { get; set; }

        public BasicAnswer()
        {
        }

        public BasicAnswer(Answer answerModel)
        {
            Id = answerModel.Id;
            AnswerBody = answerModel.AnswerText;
        }
    }
}
