using QuestionAppLibrary.Models;

namespace QuestionAppLibrary.DataAccess
{
    public interface ICategoryData
    {
        Task CreateCategory(Category category);

        Task<List<Category>> GetAllCategories();
    }
}
