using QuestionAppLibrary.Models;

namespace QuestionApp.Application.DataGenerator.Model
{
    public sealed class SeedData
    {
        public List<User> Users { get; init; } = [];
        public List<Category> Categories { get; init; } = [];
        public List<Status> Statuses { get; init; } = [];
        public List<QuestionAppLibrary.Models.Question> Questions { get; init; } = [];
        public List<Answer> Answers { get; init; } = [];
    }
}
