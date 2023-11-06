using Microsoft.Extensions.Configuration;

namespace QuestionAppLibrary.DataAccess
{
    public class DbConnection : IDbConnection
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
        public IMongoCollection<CategoryModel> CategoryCollection { get; private set; }
        public IMongoCollection<StatusModel> StatusCollection { get; private set; }
        public IMongoCollection<UserModel> UserCollection { get; private set; }
        public IMongoCollection<QuestionModel> QuestionCollection { get; private set; }
        public IMongoCollection<AnswerModel> AnswerCollection { get; private set; }

        public DbConnection(IConfiguration config)
        {
            this._config = config;
            var settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString(_connectionId));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            Client = new MongoClient(settings);
            DbName = _config["DatabaseName"];
            _db = Client.GetDatabase(DbName);

            CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
            StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
            UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
            QuestionCollection = _db.GetCollection<QuestionModel>(QuestionCollectionName);
            AnswerCollection = _db.GetCollection<AnswerModel>(AnswerCollectionName);
        }
    }
}

