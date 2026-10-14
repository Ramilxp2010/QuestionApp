using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using QuestionAppLibrary.DataAccess;
namespace QuestionsApp.Infrastructure.MongoDb.DataAccess
{
    public class MongoCategoryData : ICategoryData
    {
        private readonly IMongoCollection<CategoryDocument> _categories;

        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;

        private const string CacheName = "CategoryData";

        public MongoCategoryData(DbConnection db, IMemoryCache cache, IMapper mapper)
        {
            this._categories = db.CategoryCollection;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var output = _cache.Get<List<CategoryDocument>>(CacheName);
            if (output != null && output.Any())
                return _mapper.Map<List<Category>>(output);

            var results = await _categories.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromDays(1));

            return _mapper.Map<List<Category>>(output);
        }

        public Task CreateCategory(Category category)
        {
            var mongoDbModel = _mapper.Map<CategoryDocument>(category);
            return _categories.InsertOneAsync(mongoDbModel);
        }
    }
}
