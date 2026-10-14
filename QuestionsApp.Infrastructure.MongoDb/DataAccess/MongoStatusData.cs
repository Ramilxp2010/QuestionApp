using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using QuestionAppLibrary.DataAccess;
namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    public class MongoStatusData : IStatusData
    {
        private readonly IMongoCollection<StatusDocument> _statuses;

        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;

        private const string CacheName = "StatusData";

        public MongoStatusData(DbConnection db, IMemoryCache cache, IMapper mapper)
        {
            this._statuses = db.StatusCollection;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            var output = _cache.Get<List<StatusDocument>>(CacheName);
            if (output != null && output.Any())
                return _mapper.Map<List<Status>>(output);

            var results = await _statuses.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromDays(1));

            return _mapper.Map<List<Status>>(output);
        }

        public Task CreateStatus(Status status)
        {
            var mongoDbModel = _mapper.Map<StatusDocument>(status);
            return _statuses.InsertOneAsync(mongoDbModel);
        }
    }
}
