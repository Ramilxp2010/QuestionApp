using QuestionAppLibrary.Models;

namespace QuestionAppLibrary.DataAccess
{
    public interface IUserData
    {
        Task CreateUser(User userModel);
        Task<User> GetUser(string id);
        Task<User> GetUserByIdentifier(string objectId);
        Task<User> GetUserByUserName(string userName);
        Task<List<User>> GetUsersAsync();
        Task UpdateUser(User userModel);
    }
}
