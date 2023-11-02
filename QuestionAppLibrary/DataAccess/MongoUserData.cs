namespace QuestionAppLibrary.DataAccess
{
    public class MongoUserData : IUserData
    {
        private readonly IMongoCollection<UserModel> _users;

        public MongoUserData(IDbConnection db)
        {
            _users = db.UserCollection;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var results = await _users.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<UserModel> GetUser(string id)
        {
            var result = await _users.FindAsync(x => x.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<UserModel> GetUserByIdentifier(string objectId)
        {
            var result = await _users.FindAsync(x => x.ObjectIdentifier == objectId);
            return result.FirstOrDefault();
        }

        public Task CreateUser(UserModel userModel)
        {
            return _users.InsertOneAsync(userModel);
        }

        public Task UpdateUser(UserModel userModel)
        {
            var filter = Builders<UserModel>.Filter.Eq("Id", userModel.Id);
            return _users.ReplaceOneAsync(filter, userModel, new ReplaceOptions { IsUpsert = true });
        }
    }
}
