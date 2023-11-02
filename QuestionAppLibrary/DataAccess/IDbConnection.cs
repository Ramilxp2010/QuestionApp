using MongoDB.Driver;

namespace QuestionAppLibrary.DataAccess
{
    public interface IDbConnection
    {
        IMongoCollection<CategoryModel> CategoryCollection { get; }
        string CategoryCollectionName { get; }
        MongoClient Client { get; }
        string DbName { get; }
        IMongoCollection<QuestionModel> QuestionCollection { get; }
        string QuestionCollectionName { get; }
        IMongoCollection<StatusModel> StatusCollection { get; }
        string StatusCollectionName { get; }
        IMongoCollection<UserModel> UserCollection { get; }
        string UserCollectionName { get; }
    }
}