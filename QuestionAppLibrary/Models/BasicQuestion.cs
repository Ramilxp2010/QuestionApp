namespace QuestionAppLibrary.Models
{
    public class BasicQuestion
    {
        public string Id { get; set; }

        public string QuestionBody { get; set; }

        public BasicQuestion()
        {

        }

        public BasicQuestion(Question question)
        {
            Id = question.Id;
            QuestionBody = question.QuestionText;
        }
    }
}
