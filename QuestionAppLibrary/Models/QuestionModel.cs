namespace QuestionAppLibrary.Models
{
    public class QuestionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Question { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public CategoryModel Category { get; set; }

        public StatusModel Status { get; set; }

        public BasicUserModel Author { get; set; }

        public HashSet<string> UserVotes { get; set; } = new();

        public string Answer { get; set; }

        public bool IsAproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsArchived { get; set; }
    }
}
