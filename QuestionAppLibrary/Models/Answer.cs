namespace QuestionAppLibrary.Models
{
    public class Answer
    {
        public string Id { get; set; }

        public string AnswerText { get; set; }

        public User Author { get; set; }

        public DateTime CreateDate { get; set; }

        public string QuestionId { get; set; }

        public HashSet<string> Votes { get; set; }

        public bool IsApproved { get; set; }
    }
}
