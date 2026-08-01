namespace QuestionAppLibrary.DataAccess
{
    public interface IUserData
    {
        Task CreateUser(UserModel userModel);
        Task<UserModel> GetUser(string id);
        Task<UserModel> GetUserByIdentifier(string objectId);
        Task<UserModel> GetUserByUserName(string userName);
        Task<List<UserModel>> GetUsersAsync();
        Task UpdateUser(UserModel userModel);
    }
}
