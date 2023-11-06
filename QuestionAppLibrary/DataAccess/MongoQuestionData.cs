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
            var output = _cache.Get<List<QuestionModel>>(CacheName);
            if (output != null && output.Any())
                return output;

            var results = await _questions.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

            return output;
        }

        public async Task<List<QuestionModel>> GetAllApprovedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsAproved).ToList();
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
                && x.Categories.Any(x=>x.Id == categoryId)).ToList();
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
            var client = _db.Client;
            using var session = await client.StartSessionAsync();
            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var questionInTransaction = db.GetCollection<QuestionModel>(_db.QuestionCollectionName);
                await questionInTransaction.InsertOneAsync(question);

                var userInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUser(question.Author.Id);
                user.VotedQuestions.Add(new BasicQuestionModel(question));
                await userInTransaction.ReplaceOneAsync(x => x.Id == user.Id, user);

                //await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task UpdateQuestion(QuestionModel question)
        {
            var filter = Builders<QuestionModel>.Filter.Eq("Id", question.Id);
            await _questions.ReplaceOneAsync(filter, question, new ReplaceOptions { IsUpsert = true });
            _cache.Remove(CacheName);
        }

        public async Task UpVoeted(string questionId, string userId)
        {
            var client = _db.Client;
            using var session = await client.StartSessionAsync();
            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var questionInTransaction = db.GetCollection<QuestionModel>(_db.QuestionCollectionName);
                var question = (await questionInTransaction.FindAsync(s => s.Id == questionId)).First();

                bool isUpvote = question.UserVotes.Add(userId);
                if (!isUpvote)
                {
                    question.UserVotes.Remove(userId);
                }

                await questionInTransaction.ReplaceOneAsync(session, x => x.Id == questionId, question);

                var userInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUser(userId);

                if (isUpvote)
                {
                    user.VotedQuestions.Add(new BasicQuestionModel(question));
                }
                else
                {
                    var basicQuestion = user.VotedQuestions.First(x => x.Id == questionId);
                    user.VotedQuestions.Remove(basicQuestion);
                }
                await userInTransaction.ReplaceOneAsync(session, x => x.Id == userId, user);

                //await session.CommitTransactionAsync();

                _cache.Remove(CacheName);
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                throw;
            }
        }

    }
}
