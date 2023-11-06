namespace QuestionAppLibrary.DataAccess
{
    public class MongoAnswerData : IAnswerData
    {
        private readonly IMongoCollection<AnswerModel> _answerCollection;

        public MongoAnswerData(IDbConnection db)
        {
            this._answerCollection = db.AnswerCollection;
        }

        public async Task<List<AnswerModel>> GetQuestionAnswers(string questionId)
        {
            var results = await _answerCollection.FindAsync(x => x.QuestionId == questionId);
            return results.ToList();
        }

        public async Task<List<AnswerModel>> GetUserAnswers(string userId)
        {
            var results = await _answerCollection.FindAsync(x => x.Author.Id == userId);
            return results.ToList();
        }
        
        public async Task<AnswerModel> GetUserAnswer(string questionId, string userId)
        {
            var results = await GetQuestionAnswers(questionId);
            return results.FirstOrDefault(x=> x.Author.Id == userId);
        }

        public Task CreateAnswer(AnswerModel answerModel)
        {
            return _answerCollection.InsertOneAsync(answerModel);
        }

        public Task UpdateAnswer(AnswerModel answerModel)
        {
            var filter = Builders<AnswerModel>.Filter.Eq("Id", answerModel.Id);
            return _answerCollection.ReplaceOneAsync(filter, answerModel, new ReplaceOptions { IsUpsert = true });
        }
    }
}
