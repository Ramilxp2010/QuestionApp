namespace QuestionAppLibrary.DataAccess
{
    public interface IQuestionData
    {
        Task CreateQuestion(QuestionModel question);
        Task<List<QuestionModel>> GetAllApprovedQuestions();
        Task<List<QuestionModel>> GetAllArchivedQuestions();
        Task<List<QuestionModel>> GetAllQuestions();
        Task<List<QuestionModel>> GetAllQuestionsByAuthor(string authorId);
        Task<List<QuestionModel>> GetAllQuestionsByCategoty(string categoryId);
        Task<List<QuestionModel>> GetAllQuestionsByStatus(string statusId);
        Task<List<QuestionModel>> GetAllQuestionsByUserVotes(string userId);
        Task<List<QuestionModel>> GetAllQuestionsForApprove();
        Task<List<QuestionModel>> GetAllRejectedQuestions();
        Task<QuestionModel> GetQuestion(string id);
        Task UpdateQuestion(QuestionModel question);
        Task UpVoeted(string questionId, string userId);
    }
}