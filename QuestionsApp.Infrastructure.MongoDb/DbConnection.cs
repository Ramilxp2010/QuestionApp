namespace QuestionsApp.Infrastructure.MongoDb
{
    public class DbConnection
    {
        public string CategoryCollectionName { get; private set; } = "categories";
        public string StatusCollectionName { get; private set; } = "statuses";
        public string UserCollectionName { get; private set; } = "users";
        public string QuestionCollectionName { get; private set; } = "questions";
        public string AnswerCollectionName { get; private set; } = "answers";

        public DbConnection(IMongoDatabase db)
        {
            CategoryCollection = db.GetCollection<CategoryDocument>(CategoryCollectionName);
            StatusCollection = db.GetCollection<StatusDocument>(StatusCollectionName);
            UserCollection = db.GetCollection<UserDocument>(UserCollectionName);
            QuestionCollection = db.GetCollection<QuestionDocument>(QuestionCollectionName);
            AnswerCollection = db.GetCollection<AnswerDocument>(AnswerCollectionName);
        }

        public IMongoCollection<CategoryDocument> CategoryCollection { get; private set; }
        public IMongoCollection<StatusDocument> StatusCollection { get; private set; }
        public IMongoCollection<UserDocument> UserCollection { get; private set; }
        public IMongoCollection<QuestionDocument> QuestionCollection { get; private set; }
        public IMongoCollection<AnswerDocument> AnswerCollection { get; private set; }

    }
}

