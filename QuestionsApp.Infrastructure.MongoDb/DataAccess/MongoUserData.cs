using MapsterMapper;
using QuestionAppLibrary.DataAccess;

namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    public class MongoUserData : IUserData
    {
        private readonly IMongoCollection<UserDocument> _users;

        private readonly IMapper _mapper;

        public MongoUserData(DbConnection db, IMapper mapper)
        {
            _users = db.UserCollection;
            _mapper = mapper;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var results = await _users
                .Find(_ => true)
                .ToListAsync();

            return _mapper.Map<List<User>>(results);
        }

        public async Task<User> GetUser(string id)
        {
            var result = await _users
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<User>(result);
        }

        public async Task<User> GetUserByIdentifier(string objectId)
        {
            var result = await _users
                .Find(x => x.ObjectIdentifier == objectId)
                .FirstOrDefaultAsync();

            return _mapper.Map<User>(result);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var result = await _users
                .Find(x => x.UserName == userName)
                .FirstOrDefaultAsync();

            return _mapper.Map<User>(result);
        }

        public Task CreateUser(User userModel)
        {
            var model = _mapper.Map<UserDocument>(userModel);
            return _users.InsertOneAsync(model);
        }

        public Task UpdateUser(User userModel)
        {
            var model = _mapper.Map<UserDocument>(userModel);
            return _users.ReplaceOneAsync(x => x.Id == userModel.Id, model);
        }
    }
}
