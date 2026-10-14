using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using QuestionAppLibrary.DataAccess;

namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    public class MongoQuestionData : IQuestionData
    {
        private readonly IMongoCollection<QuestionDocument> _questions;

        private readonly IMapper _mapper;

        private readonly IUserData _userData;

        private readonly IMemoryCache _cache;

        private const string CacheName = "QuestionData";

        public MongoQuestionData(DbConnection db, IUserData userData, IMemoryCache cache, IMapper mapper)
        {
            _questions = db.QuestionCollection;
            this._userData = userData;
            this._cache = cache;
            _mapper = mapper;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            var cached = _cache.Get<List<QuestionDocument>>(CacheName);
            if (cached == null || !cached.Any())
            {
                var results = await _questions.FindAsync(_ => true);
                cached = results.ToList();
                _cache.Set(CacheName, cached, TimeSpan.FromMinutes(1));
            }

            // Return copies so UI mutations do not dirty the shared cache.
            var result = cached.Select(CloneQuestion).ToList();

            return _mapper.Map<List<Question>>(result);
        }

        public async Task<List<Question>> GetAllApprovedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsApproved && !x.IsArchived && !x.IsRejected).ToList();
        }

        public async Task<List<Question>> GetAllRejectedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsRejected).ToList();
        }

        public async Task<List<Question>> GetAllArchivedQuestions()
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsArchived).ToList();
        }

        public async Task<List<Question>> GetAllQuestionsByCategory(string categoryId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsApproved
                && !x.IsRejected
                && !x.IsArchived
                && x.Categories.Any(c => c.Id == categoryId)).ToList();
        }

        public async Task<List<Question>> GetAllQuestionsByStatus(string statusId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.IsApproved
                && !x.IsRejected
                && !x.IsArchived
                && x.Status.Id == statusId).ToList();
        }

        public async Task<List<Question>> GetAllQuestionsForApprove()
        {
            var results = await GetAllQuestions();
            return results.Where(x => !x.IsApproved
                && !x.IsRejected
                && !x.IsArchived).ToList();
        }

        public async Task<List<Question>> GetAllQuestionsByAuthor(string authorId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.Author.Id == authorId).ToList();
        }

        public async Task<List<Question>> GetAllQuestionsByUserVotes(string userId)
        {
            var results = await GetAllQuestions();
            return results.Where(x => x.UserVotes.Contains(userId)).ToList();
        }

        public async Task<Question> GetQuestion(string id)
        {
            var result = await _questions.FindAsync(x => x.Id == id);
            return _mapper.Map<Question>(result.FirstOrDefault());
        }

        public async Task CreateQuestion(Question question)
        {
            var mongoDbModel = _mapper.Map<QuestionDocument>(question);
            await _questions.InsertOneAsync(mongoDbModel);

            var user = await _userData.GetUser(question.Author.Id);
            if (user != null)
            {
                user.AuthoredQuestions.Add(new BasicQuestion(question));
                await _userData.UpdateUser(user);
            }

            _cache.Remove(CacheName);
        }

        public async Task UpdateQuestion(Question question)
        {
            var mongoDbModel = _mapper.Map<QuestionDocument>(question);
            await _questions.ReplaceOneAsync(x => x.Id == question.Id, mongoDbModel);
            _cache.Remove(CacheName);
        }

        public async Task UpVoted(string questionId, string userId)
        {
            var result = await _questions.FindAsync(x => x.Id == questionId);

            var question = await result.FirstOrDefaultAsync();
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
                    var model = _mapper.Map<Question>(question);
                    user.VotedQuestions.Add(new BasicQuestion(model));
                }
            }
            else
            {
                user.VotedQuestions.RemoveAll(x => x.Id == questionId);
            }

            await _userData.UpdateUser(user);
            _cache.Remove(CacheName);
        }

        private static QuestionDocument CloneQuestion(QuestionDocument source)
        {
            return new QuestionDocument
            {
                Id = source.Id,
                QuestionText = source.QuestionText,
                Description = source.Description,
                DateCreated = source.DateCreated,
                Categories = source.Categories?.ToList(),
                Status = source.Status,
                Author = source.Author,
                UserVotes = source.UserVotes != null ? new HashSet<string>(source.UserVotes) : new HashSet<string>(),
                Answer = source.Answer,
                IsApproved = source.IsApproved,
                IsRejected = source.IsRejected,
                IsArchived = source.IsArchived
            };
        }
    }
}
