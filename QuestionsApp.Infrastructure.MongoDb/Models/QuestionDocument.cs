namespace QuestionAppLibrary.Models
{
    public class QuestionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string QuestionText { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public List<CategoryDocument> Categories { get; set; }

        public StatusDocument Status { get; set; }

        public BasicUserDocument Author { get; set; }

        public HashSet<string> UserVotes { get; set; } = new();

        public string Answer { get; set; }

        [BsonElement("IsAproved")]
        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsArchived { get; set; }
    }
}
