using Microsoft.AspNetCore.Identity;
using QuestionAppUI.Data;

namespace QuestionAppUI.Helpers
{
    public static class DemoDataSeeder
    {
        public static async Task EnsureDemoDataAsync(
            IUserData userData,
            IPasswordHasher<UserModel> passwordHasher,
            ICategoryData categoryData,
            IStatusData statusData,
            IQuestionData questionData,
            IAnswerData answerData)
        {
            await DemoUserSeeder.EnsureDemoUsersAsync(userData, passwordHasher);
            await EnsureCategoriesAsync(categoryData);
            await EnsureStatusesAsync(statusData);
            await EnsureQuestionsAsync(userData, categoryData, statusData, questionData);
            await EnsureAnswersAsync(userData, questionData, answerData);
        }

        private static async Task EnsureCategoriesAsync(ICategoryData categoryData)
        {
            var categories = await categoryData.GetAllCategories();
            if (categories != null && categories.Count > 0)
            {
                return;
            }

            foreach (var category in SampleData.SampleCategories)
            {
                await categoryData.CreateCategory(category);
            }
        }

        private static async Task EnsureStatusesAsync(IStatusData statusData)
        {
            var statuses = await statusData.GetAllStatuses();
            if (statuses != null && statuses.Count > 0)
            {
                return;
            }

            foreach (var status in SampleData.SampleStatuses)
            {
                await statusData.CreateStatus(status);
            }
        }

        private static async Task EnsureQuestionsAsync(
            IUserData userData,
            ICategoryData categoryData,
            IStatusData statusData,
            IQuestionData questionData)
        {
            var existing = await questionData.GetAllQuestions();
            if (existing != null && existing.Count > 0)
            {
                return;
            }

            var categories = await categoryData.GetAllCategories();
            var statuses = await statusData.GetAllStatuses();
            var defaultStatus = statuses?.FirstOrDefault(s =>
                    string.Equals(s.Name, "watching", StringComparison.OrdinalIgnoreCase))
                ?? statuses?.FirstOrDefault();

            foreach (var template in SampleData.SampleQuestions)
            {
                var author = await userData.GetUserByIdentifier(template.Author?.ObjectId);
                if (author == null)
                {
                    continue;
                }

                var question = new QuestionModel
                {
                    Question = template.Question,
                    Description = template.Description,
                    Answer = template.Answer,
                    DateCreated = template.DateCreated == default ? DateTime.UtcNow : template.DateCreated,
                    Author = new BasicUserModel(author),
                    Categories = categories?
                        .Where(c => template.Categories != null && template.Categories.Any(tc => tc.Name == c.Name))
                        .ToList() ?? new List<CategoryModel>(),
                    Status = defaultStatus,
                    IsApproved = true,
                    IsRejected = false,
                    IsArchived = false,
                    UserVotes = new HashSet<string>()
                };

                await questionData.CreateQuestion(question);
            }
        }

        private static async Task EnsureAnswersAsync(
            IUserData userData,
            IQuestionData questionData,
            IAnswerData answerData)
        {
            var questions = await questionData.GetAllQuestions();
            if (questions == null || questions.Count == 0)
            {
                return;
            }

            var firstQuestionAnswers = await answerData.GetQuestionAnswers(questions[0].Id);
            if (firstQuestionAnswers != null && firstQuestionAnswers.Count > 0)
            {
                return;
            }

            foreach (var question in questions)
            {
                foreach (var template in SampleData.SampleAnswers)
                {
                    var author = await userData.GetUserByIdentifier(template.Author?.ObjectIdentifier);
                    if (author == null)
                    {
                        continue;
                    }

                    var answer = new AnswerModel
                    {
                        Answer = template.Answer,
                        Author = author,
                        CreateDate = DateTime.UtcNow,
                        QuestionId = question.Id,
                        Votes = new HashSet<string>(),
                        IsApproved = true
                    };

                    await answerData.CreateAnswer(answer);
                }
            }
        }
    }
}
