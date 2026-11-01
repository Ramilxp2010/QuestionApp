using MapsterMapper;
using QuestionApp.Application.DataGenerator;
using QuestionApp.Application.DataGenerator.Model;

namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    /// <summary>
    /// Массовая запись тестовых данных через InsertMany: одна операция на коллекцию
    /// вместо тысяч InsertOne/ReplaceOne, как было бы через IQuestionData/IAnswerData.
    /// </summary>
    public sealed class MongoSeedDataStore : ISeedDataStore
    {
        private readonly DbConnection _db;
        private readonly IMapper _mapper;

        public MongoSeedDataStore(DbConnection db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> IsEmptyAsync(CancellationToken cancellationToken = default)
        {
            var hasData = await Task.WhenAll(
                HasAnyAsync(_db.UserCollection, cancellationToken),
                HasAnyAsync(_db.CategoryCollection, cancellationToken),
                HasAnyAsync(_db.StatusCollection, cancellationToken),
                HasAnyAsync(_db.QuestionCollection, cancellationToken),
                HasAnyAsync(_db.AnswerCollection, cancellationToken));

            return !hasData.Any(x => x);
        }

        public async Task SaveAsync(SeedData data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(data);

            var questions = _mapper.Map<List<QuestionDocument>>(data.Questions);
            foreach (var question in questions)
            {
                // В доменной модели VotesCount нет, а GetApprovedQuestions сортирует по нему.
                question.VotesCount = question.UserVotes?.Count ?? 0;
            }

            // Без транзакции: standalone MongoDB их не поддерживает.
            // При сбое посередине базу проще удалить и запустить заново.
            await InsertAsync(_db.CategoryCollection, _mapper.Map<List<CategoryDocument>>(data.Categories), cancellationToken);
            await InsertAsync(_db.StatusCollection, _mapper.Map<List<StatusDocument>>(data.Statuses), cancellationToken);
            await InsertAsync(_db.UserCollection, _mapper.Map<List<UserDocument>>(data.Users), cancellationToken);
            await InsertAsync(_db.QuestionCollection, questions, cancellationToken);
            await InsertAsync(_db.AnswerCollection, _mapper.Map<List<AnswerDocument>>(data.Answers), cancellationToken);
        }

        private static async Task<bool> HasAnyAsync<T>(IMongoCollection<T> collection, CancellationToken cancellationToken)
        {
            var count = await collection.CountDocumentsAsync(
                FilterDefinition<T>.Empty,
                new CountOptions { Limit = 1 },
                cancellationToken);

            return count > 0;
        }

        private static Task InsertAsync<T>(IMongoCollection<T> collection, List<T> documents, CancellationToken cancellationToken)
        {
            return documents.Count == 0
                ? Task.CompletedTask
                : collection.InsertManyAsync(documents, new InsertManyOptions { IsOrdered = false }, cancellationToken);
        }
    }
}
