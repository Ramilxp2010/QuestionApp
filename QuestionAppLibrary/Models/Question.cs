namespace QuestionAppLibrary.Models
{
    public class Question
    {
        public string Id { get; set; }

        public string QuestionText { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public List<Category> Categories { get; set; }

        public Status Status { get; set; }

        public BasicUser Author { get; set; }

        public HashSet<string> UserVotes { get; set; } = new();

        public string Answer { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsArchived { get; set; }
    }
}
