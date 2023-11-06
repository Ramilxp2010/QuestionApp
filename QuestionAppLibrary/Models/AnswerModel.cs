namespace QuestionAppLibrary.Models
{
    public class AnswerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Answer { get; set; }

        public UserModel Author { get; set; }

        public string QuestionId { get; set; }

        public HashSet<string> Votes { get; set; }

        public bool IsApproved { get; set; }
    }
}
