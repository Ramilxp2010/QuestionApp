﻿using Microsoft.Extensions.Caching.Memory;
namespace QuestionAppLibrary.DataAccess
{
    public class MongoStatusData : IStatusData
    {
        private readonly IMongoCollection<StatusModel> _statuses;
        private readonly IMemoryCache _cache;
        private const string CacheName = "StatusData";

        public MongoStatusData(IDbConnection db, IMemoryCache cache)
        {
            this._statuses = db.StatusCollection;
            _cache = cache;
        }

        public async Task<List<StatusModel>> GetAllStatuses()
        {
            var output = _cache.Get<List<StatusModel>>(CacheName);
            if (output != null && output.Any())
                return output;

            var results = await _statuses.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromDays(1));

            return output;
        }

        public Task CreateStatus(StatusModel status)
        {
            return _statuses.InsertOneAsync(status);
        }
    }
}
