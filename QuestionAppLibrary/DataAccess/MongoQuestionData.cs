using Microsoft.Extensions.Caching.Memory;

namespace QuestionAppLibrary.DataAccess
{
    public class MongoQuestionData : IQuestionData
    {
        private readonly IMongoCollection<QuestionModel> _questions;
        private readonly IDbConnection _db;
        private readonly IUserData _userData;
        private readonly IMemoryCache _cache;
        private const string CacheName = "QuestionData";

        public MongoQuestionData(IDbConnection db, IUserData userData, IMemoryCache cache)
        {
            _questions = db.QuestionCollection;
            this._db = db;
            this._userData = userData;
            this._cache = cache;
        }

        public async Task<List<QuestionModel>> GetAllQuestions()
        {
            var cached = _cache.Get<List<QuestionModel>>(CacheName);
            if (cached == null || !cached.Any())
            {
                var results = await _questions.FindAsync(_ => true);
                cached = results.ToList();
                _cache.Set(CacheName, cached, TimeSpan.FromMinutes(1));
            }

            // Return copies so UI mutations do not dirty the shared cache.
            return cached.Select(CloneQuestion).ToList();
        }

        public async Task<List<QuestionModel>> GetAllApprovedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsAproved && !x.IsArchived && !x.IsRejected).ToList();
        }

        public async Task<List<QuestionModel>> GetAllRejectedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsRejected).ToList();
        }

        public async Task<List<QuestionModel>> GetAllArchivedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsArchived).ToList();
        }

        public async Task<List<QuestionModel>> GetAllQuestionsByCategoty(string categoryId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsAproved
                && !x.IsRejected
                && !x.IsArchived
                && x.Categories.Any(c => c.Id == categoryId)).ToList();
        }

        public async Task<List<QuestionModel>> GetAllQuestionsByStatus(string statusId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsAproved
                && !x.IsRejected
                && !x.IsArchived
                && x.Status.Id == statusId).ToList();
        }

        public async Task<List<QuestionModel>> GetAllQuestionsForApprove()
        {
            var results = await GetAllQuestions();
            return results.Where(x => !x.IsAproved
                && !x.IsRejected
                && !x.IsArchived).ToList();
        }

        public async Task<List<QuestionModel>> GetAllQuestionsByAuthor(string authorId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.Author.Id == authorId).ToList();
        }

        public async Task<List<QuestionModel>> GetAllQuestionsByUserVotes(string userId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.UserVotes.Contains(userId)).ToList();
        }

        public async Task<QuestionModel> GetQuestion(string id)
        {
            var result = await _questions.FindAsync(x => x.Id == id);
            return result.FirstOrDefault();
        }

        public async Task CreateQuestion(QuestionModel question)
        {
            await _questions.InsertOneAsync(question);

            var user = await _userData.GetUser(question.Author.Id);
            if (user != null)
            {
                user.AutoredQuestions.Add(new BasicQuestionModel(question));
                await _userData.UpdateUser(user);
            }

            _cache.Remove(CacheName);
        }

        public async Task UpdateQuestion(QuestionModel question)
        {
            await _questions.ReplaceOneAsync(x => x.Id == question.Id, question);
            _cache.Remove(CacheName);
        }

        public async Task UpVoeted(string questionId, string userId)
        {
            var question = await GetQuestion(questionId);
            var user = await _userData.GetUser(userId);
            if (question == null || user == null)
            {
                return;
            }

            bool isUpvote = question.UserVotes.Add(userId);
            if (!isUpvote)
            {
                question.UserVotes.Remove(userId);
            }

            await _questions.ReplaceOneAsync(x => x.Id == questionId, question);

            if (isUpvote)
            {
                if (!user.VotedQuestions.Any(x => x.Id == questionId))
                {
                    user.VotedQuestions.Add(new BasicQuestionModel(question));
                }
            }
            else
            {
                user.VotedQuestions.RemoveAll(x => x.Id == questionId);
            }

            await _userData.UpdateUser(user);
            _cache.Remove(CacheName);
        }

        private static QuestionModel CloneQuestion(QuestionModel source)
        {
            return new QuestionModel
            {
                Id = source.Id,
                Question = source.Question,
                Description = source.Description,
                DateCreated = source.DateCreated,
                Categories = source.Categories?.ToList(),
                Status = source.Status,
                Author = source.Author,
                UserVotes = source.UserVotes != null ? new HashSet<string>(source.UserVotes) : new HashSet<string>(),
                Answer = source.Answer,
                IsAproved = source.IsAproved,
                IsRejected = source.IsRejected,
                IsArchived = source.IsArchived
            };
        }
    }
}
