using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionsApp.Infrastructure.MongoDb.Init
{
    public class MongoDbInitializer
    {
        private readonly IMongoDatabase _database;

        public MongoDbInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync(
            CancellationToken cancellationToken = default)
        {
            var questions = _database.GetCollection<QuestionDocument>("questions");

            var indexes = new List<CreateIndexModel<QuestionDocument>>
            {
                new(
                    Builders<QuestionDocument>.IndexKeys
                        .Ascending(x => x.IsApproved)
                        .Descending(x => x.UserVotes)
                        .Descending(x=>x.DateCreated)
                        .Ascending(x=>x.Id),
                    new CreateIndexOptions
                    {
                        Name = "approved_votes_date_id"
                    }),
                new(
                    Builders<QuestionDocument>.IndexKeys
                        .Ascending("Categories._id"),
                    new CreateIndexOptions
                    {
                        Name = "categories_id"
                    }),
                new(
                    Builders<QuestionDocument>.IndexKeys
                        .Ascending("Status._id"),
                    new CreateIndexOptions
                    {
                        Name = "status_id"
                    }),
                new(
                    Builders<QuestionDocument>.IndexKeys
                        .Text(x=>x.QuestionText)
                        .Text(x=>x.Description),
                    new CreateIndexOptions
                    {
                        Name = "question_text_search"
                    })
            };

            await questions.Indexes.CreateManyAsync(indexes, cancellationToken);
        }
    }
}
