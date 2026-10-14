using QuestionAppLibrary.Models;

namespace QuestionAppLibrary.DataAccess
{
    public interface IAnswerData
    {
        Task CreateAnswer(Answer answerModel);

        Task<List<Answer>> GetQuestionAnswers(string questionId);

        Task<List<Answer>> GetUserAnswers(string userId);

        Task<Answer> GetUserAnswer(string questionId, string userId);

        Task UpdateAnswer(Answer answerModel);
    }
}