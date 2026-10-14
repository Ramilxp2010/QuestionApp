namespace QuestionAppLibrary.Models
{
    public class AnswerDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AnswerText { get; set; }

        public UserDocument Author { get; set; }

        public DateTime CreateDate { get; set; }

        public string QuestionId { get; set; }

        public HashSet<string> Votes { get; set; }

        public bool IsApproved { get; set; }
    }
}
