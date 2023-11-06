namespace QuestionAppLibrary.DataAccess
{
    public interface IAnswerData
    {
        Task CreateAnswer(AnswerModel answerModel);

        Task<List<AnswerModel>> GetQuestionAnswers(string questionId);

        Task<List<AnswerModel>> GetUserAnswers(string userId);

        Task<AnswerModel> GetUserAnswer(string questionId, string userId);

        Task UpdateAnswer(AnswerModel answerModel);
    }
}