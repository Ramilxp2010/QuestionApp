using QuestionAppLibrary.Models;

namespace QuestionAppLibrary.DataAccess
{
    public interface IStatusData
    {
        Task CreateStatus(Status status);
        Task<List<Status>> GetAllStatuses();
    }
}