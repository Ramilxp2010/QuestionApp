using Microsoft.Extensions.Configuration;

namespace QuestionsApp.Infrastructure.MongoDb
{
    public class DbConnection
    {
        private readonly IConfiguration _config;
        private readonly IMongoDatabase _db;
        private string _connectionId = "MongoDB";

        public string DbName { get; private set; }
        public string CategoryCollectionName { get; private set; } = "categories";
        public string StatusCollectionName { get; private set; } = "statuses";
        public string UserCollectionName { get; private set; } = "users";
        public string QuestionCollectionName { get; private set; } = "questions";
        public string AnswerCollectionName { get; private set; } = "answers";

        public MongoClient Client { get; private set; }
        public IMongoCollection<CategoryDocument> CategoryCollection { get; private set; }
        public IMongoCollection<StatusDocument> StatusCollection { get; private set; }
        public IMongoCollection<UserDocument> UserCollection { get; private set; }
        public IMongoCollection<QuestionDocument> QuestionCollection { get; private set; }
        public IMongoCollection<AnswerDocument> AnswerCollection { get; private set; }

        public DbConnection(IConfiguration config)
        {
            this._config = config;
            var settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString(_connectionId));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            Client = new MongoClient(settings);
            DbName = _config["DatabaseName"];
            _db = Client.GetDatabase(DbName);

            CategoryCollection = _db.GetCollection<CategoryDocument>(CategoryCollectionName);
            StatusCollection = _db.GetCollection<StatusDocument>(StatusCollectionName);
            UserCollection = _db.GetCollection<UserDocument>(UserCollectionName);
            QuestionCollection = _db.GetCollection<QuestionDocument>(QuestionCollectionName);
            AnswerCollection = _db.GetCollection<AnswerDocument>(AnswerCollectionName);
        }
    }
}

