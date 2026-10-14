using QuestionAppLibrary.Models;

namespace QuestionAppLibrary.DataAccess
{
    public interface IQuestionData
    {
        Task CreateQuestion(Question question);
        Task<List<Question>> GetAllApprovedQuestions();
        Task<List<Question>> GetAllArchivedQuestions();
        Task<List<Question>> GetAllQuestions();
        Task<List<Question>> GetAllQuestionsByAuthor(string authorId);
        Task<List<Question>> GetAllQuestionsByCategory(string categoryId);
        Task<List<Question>> GetAllQuestionsByStatus(string statusId);
        Task<List<Question>> GetAllQuestionsByUserVotes(string userId);
        Task<List<Question>> GetAllQuestionsForApprove();
        Task<List<Question>> GetAllRejectedQuestions();
        Task<Question> GetQuestion(string id);
        Task UpdateQuestion(Question question);
        Task UpVoted(string questionId, string userId);
    }
}
