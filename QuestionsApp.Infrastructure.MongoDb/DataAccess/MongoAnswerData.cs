using QuestionAppLibrary.DataAccess;
using MapsterMapper;

namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    public class MongoAnswerData : IAnswerData
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<AnswerDocument> _answerCollection;

        public MongoAnswerData(DbConnection db, IMapper mapper)
        {
            this._answerCollection = db.AnswerCollection;
            _mapper = mapper;
        }

        public async Task<List<Answer>> GetQuestionAnswers(string questionId)
        {
            var results = await _answerCollection
                .Find(x => x.QuestionId == questionId)
                .ToListAsync();

            return _mapper.Map<List<Answer>>(results);
        }

        public async Task<List<Answer>> GetUserAnswers(string userId)
        {
            var results = await _answerCollection
                .Find(x => x.Author.Id == userId)
                .ToListAsync()
                ;

            return _mapper.Map<List<Answer>>(results);
        }
        
        public async Task<Answer> GetUserAnswer(string questionId, string userId)
        {
            var result = await _answerCollection
                .Find(x => x.QuestionId == questionId && x.Author.Id == userId)
                .FirstOrDefaultAsync();

            return result == null 
                ? null 
                : _mapper.Map<Answer>(result);
        }

        public Task CreateAnswer(Answer answerModel)
        {
            var mongoDbModel = _mapper.Map<AnswerDocument>(answerModel);
            return _answerCollection.InsertOneAsync(mongoDbModel);
        }

        public Task UpdateAnswer(Answer answerModel)
        {
            var mongoDbModel = _mapper.Map<AnswerDocument>(answerModel);
            return _answerCollection.ReplaceOneAsync(x => x.Id == answerModel.Id, mongoDbModel);
        }
    }
}
